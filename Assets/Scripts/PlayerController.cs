using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public enum GameState {
		PreGame,
		Running,
		Lost,
		TrackingLost
	}
	
	public enum LevelState {
		NotStarted,
		Countdown,
		PhaseOne,
		PreWaveOne,
		WaveOne,
		PhaseTwo,
		PreWaveTwo,
		WaveTwo
	}
	
	public enum ShootingMode {
		Crosshair = 0,
		TapOver = 1
	}

	public GameState currentState = GameState.PreGame;
	private GameState previousState;
	
	public LevelState currentLevelState = LevelState.NotStarted;
	private LevelState previousLevelState;
	
	public float minSpawnDistance = 12f;
	public float maxSpawnDistance = 30f;
	public float enemySpeed = 2;
	public float killDistance = 7;
	public float jumpAngle = 50f;
	public float minJumpTargetHeight = 0f;
	public float maxJumpTargetHeight = 2f;
	
	public int startingHitPoints = 3;
	
	public GameObject enemy;
	public GameObject projectile;
	public GameObject weapon;
	public GameObject splashWhenHit;
	public GameObject splashWhenAttacking;
	
	public LayerMask enemyLayerMask;
	
	private ShootingMode shootingMode = ShootingMode.TapOver;
	
	private int hitsTaken = 0;
	private int enemiesKilled = 0;
	private int hitPoints;
	private int enemyCount = 0;
	
	private float enemySpawnRate;
	
	private float stageTimeElapsed;
	
	private bool isShooting = false;
	
	public bool debugEnabled = false;
	
	private int touches = 0;


	void Start () {
		enemySpawnRate = 0;

		StartCoroutine(SpawnEnemies());
		StartCoroutine(RunLevelStages());

		previousState = GameState.PreGame;
		isShooting = false;
		
		currentLevelState = LevelState.NotStarted;
		stageTimeElapsed = 0.0f;
		
		StartCoroutine(ConfigureCamera());
		
		AudioManager.instance.playBGM("bgm");
	}
	
	IEnumerator ConfigureCamera() {
		yield return new WaitForSeconds(1.0f);
		CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}
	
	IEnumerator ShowStartUI() {
		yield return new WaitForSeconds(0.5f);
	
		GUIController gui = GetComponent<GUIController>();
		
		gui.DisplayMessage("3");
		yield return new WaitForSeconds(1.0f);
		
		gui.DisplayMessage("2");
		yield return new WaitForSeconds(1.0f);
		
		gui.DisplayMessage("1");
		yield return new WaitForSeconds(1.0f);
		
		gui.DisplayMessage("Go!");
		yield return new WaitForSeconds(1.5f);
		
		gui.HideMessages();
	}
	
	void Update () {
		if (currentState == GameState.Running) {
			touches = Input.touchCount;
		
			// Count enemies
			enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

			// Update enemies that are too close
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
				float d = Vector3.Distance(obj.transform.position, transform.position);
				if (d < killDistance) {
					obj.tag = "";
					obj.GetComponent<Animator>().SetTrigger("Jump");
					StartCoroutine(JumpOnHero(obj, 0.42f));
				}
			}

			if (hitPoints <= 0) {
				// Trigger Game Over
				SetGameState(GameState.Lost);
			}

			// Shoot enemies if necessary
			if (!isShooting && Input.GetMouseButton(0)) {
				RaycastHit hit;
				Transform camera = Camera.main.transform;
				
				Ray ray;
				if (shootingMode == ShootingMode.Crosshair) {
					ray = new Ray(camera.position, camera.forward);
				}
				else {
					ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				}
				
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayerMask)) {
					StartCoroutine(ProcessShot(hit.collider.gameObject));
				}
			}
			
			// Move enemies
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
				float step = enemySpeed * Time.deltaTime;
				obj.transform.position = Vector3.MoveTowards(obj.transform.position, 
				                                               transform.position, step);
			}

			// Update elapsed time
			stageTimeElapsed += Time.deltaTime;
		}
	}
	
	void OnGameStateChange(GameState newState) {
		if (newState == GameState.Running && previousState == GameState.PreGame) {
			StartGame();
		}
	
		if (newState == GameState.Lost) {
			// Play losing animation
			GetComponent<Animator>().SetTrigger("Die");
			
			// Play losing sound
			AudioManager.instance.playEffect("loseLevel");
			
			// Display losing screen
			GetComponent<GUIController>().ShowGameOverScreen(GUIController.GameOverMode.Lose);
			StopCoroutine("RunLevelStages");
			
			// Destroy enemies
			DestroyAllEnemies();
		}
		
		if (newState == GameState.TrackingLost) {
			// Ask user to point camera again
			GetComponent<GUIController>().ShowARMask();
		} else {
			GetComponent<GUIController>().HideARMask();
			
			if (newState == GameState.Lost) {
				GetComponent<GUIController>().HideHUD();
			}
		}
		
		// Update previous state
		previousState = newState;
	}
	
	public void SetGameState(GameState newState) {
		currentState = newState;
		
		if (newState != previousState) {			
			OnGameStateChange(newState);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		// Save position to instantiate splash
		Vector3 thePosition = other.gameObject.transform.position;

		// Destroy the enemy
		Destroy (other.gameObject);
		
		// Play hit sound
		AudioManager.instance.playEffect("frankHurt");
		
		// Play splash sound
		AudioManager.instance.playEffect("amebaSplash");
		
		// Play random hit animation
		Animator anim = GetComponent<Animator>();
		float r = Random.value;
		anim.SetFloat("RandHit", Random.value);
		anim.SetTrigger("Hit");

		// Instantiate particle splash effect
		SplashAmeba (thePosition, false);
		
		// Update hitpoints
		hitPoints--;
		GetComponent<GUIController>().UpdateHitPoints(hitPoints);
	}

	public void SplashAmeba(Vector3 position, bool whenHit = false) {
		GameObject effect = (whenHit) ? splashWhenHit : splashWhenAttacking;
		
		GameObject splashEffect = (GameObject) Instantiate (effect);
		splashEffect.transform.position = position;
	}

	IEnumerator WaitUntilAllEnemiesKilled() {
		while (enemyCount > 0) {
			yield return null;
		}
	}

	IEnumerator WaitUntilElapsed(float t) {
		while (stageTimeElapsed < t) {
			yield return null;
		}
	}

	IEnumerator RunLevelStages() {
		GUIController gui = GetComponent<GUIController>();

		// Avoids starting the level before tracking acquired
		currentLevelState = LevelState.NotStarted;
		enemySpawnRate = 0;
		stageTimeElapsed = 0;
		yield return StartCoroutine(WaitUntilElapsed (0.1f));

		// Stage 1 - Countdown
		currentLevelState = LevelState.Countdown;
		enemySpawnRate = 0f;
		stageTimeElapsed = 0;
		StartCoroutine(ShowStartUI());
		yield return StartCoroutine(WaitUntilElapsed (5.0f));

		// Phase One
		currentLevelState = LevelState.PhaseOne;
		enemySpawnRate = 16.0f;
		yield return StartCoroutine(WaitUntilElapsed (15.0f));
		enemySpawnRate = 0;
		yield return StartCoroutine(WaitUntilAllEnemiesKilled ());
		stageTimeElapsed = 0;

		// Display first wave UI
		currentLevelState = LevelState.PreWaveOne;
		enemySpawnRate = 0f;
		gui.DisplayMessage("A wave is coming!");
		gui.HideMessages(2f);
		yield return StartCoroutine(WaitUntilElapsed (2.0f));
		
		// First wave
		currentLevelState = LevelState.WaveOne;
		enemySpawnRate = 48.0f;
		yield return StartCoroutine(WaitUntilElapsed (20.0f));
		enemySpawnRate = 0;
		yield return StartCoroutine(WaitUntilAllEnemiesKilled ());
		stageTimeElapsed = 0;

		// Phase Two
		currentLevelState = LevelState.PhaseTwo;
		enemySpawnRate = 24.0f;
		yield return StartCoroutine(WaitUntilElapsed (20.0f));
		enemySpawnRate = 0;
		yield return StartCoroutine(WaitUntilAllEnemiesKilled ());
		stageTimeElapsed = 0;

		// Final Wave UI
		currentLevelState = LevelState.PreWaveTwo;
		gui.DisplayMessage("Final wave!");
		gui.HideMessages(2f);
		yield return StartCoroutine(WaitUntilElapsed (2.0f));
		stageTimeElapsed = 0;

		// Final Wave
		currentLevelState = LevelState.WaveTwo;
		enemySpawnRate = 60f;
		yield return StartCoroutine(WaitUntilElapsed (10.0f));
		enemySpawnRate = 0;
		yield return StartCoroutine(WaitUntilAllEnemiesKilled ());
		stageTimeElapsed = 0;

		// You win!!
		WinLevel ();
	}

	void WinLevel() {
		GUIController gui = GetComponent<GUIController>();
		GetComponent<GUIController>().ShowGameOverScreen(GUIController.GameOverMode.Win);

		GetComponent<Animator> ().SetTrigger ("Win");
	}
	
	void OnGUI() {
		if (debugEnabled) {
			GUIStyle style = new GUIStyle();
			style.fontSize = 20;

			GUI.Label(new Rect(10, 10, 150, 20), "Enemy count: " + enemyCount);
			
			GUI.Label(new Rect(10, 25, 150, 20), "Hit points: " + hitPoints);
			GUI.Label(new Rect(10, 40, 150, 20), "Spawn rate: " + enemySpawnRate + " enemies per minute");
			
			if (GUI.Button(new Rect(Screen.width - 100, 10, 80, 20), "Restart")) {
				StartGame();
			}
			
			GUI.Label(new Rect(10, 55, 300, 20), "Current game state: " + currentState);
			GUI.Label(new Rect(10, 70, 300, 20), "Current level state: " + currentLevelState);
			GUI.Label(new Rect(10, 85, 300, 20), "Level time elapsed: " + stageTimeElapsed);
		}
	}
	
	// =============
	
	void StartGame() {
		GUIController gui = GetComponent<GUIController>();
	
		hitsTaken = 0;
		enemiesKilled = 0;
		hitPoints = startingHitPoints;
		gui.UpdateHitPoints(hitPoints);
		
		GetComponent<Animator>().SetTrigger("Restart");
		
		enemySpawnRate = 16f;
		
		gui.ShowHUD();
		gui.HideGameOverScreen(0f);
		
		DestroyAllEnemies();
		StartCoroutine("RunLevelStages");
	}
	
	void DestroyAllEnemies() {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
			Destroy(obj);
		}
	}
	
	IEnumerator SpawnEnemies() {
		while (true) {
			if (currentState == GameState.Running && enemySpawnRate > 0f) {
				// Generate random position
				Vector3 direction = Random.onUnitSphere;
				direction.y = 0;
				float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
				Vector3 newPosition = distance * direction.normalized;
				
				// Create new enemy
				GameObject newEnemy = Instantiate(enemy, newPosition, Quaternion.identity) as GameObject;
				newEnemy.transform.parent = transform.parent;
				
				// Play spawn sound
				AudioManager.instance.playEffect("amebaSpawn");
				
				// Make the enemy face the hero
				newEnemy.transform.LookAt(transform.position);
			}
			
			if (enemySpawnRate == 0f) {
				yield return null;
			}
			else {
				yield return new WaitForSeconds(60.0f / enemySpawnRate);
			}
		}
	}
	
	IEnumerator JumpOnHero(GameObject obj, float delay) {
		yield return new WaitForSeconds(delay);
		
		// Check if object was already destroyed by endgame
		if (obj == null) {
			return false;
		}
		
		// Determine random height to hit
		float h = Random.Range(minJumpTargetHeight, maxJumpTargetHeight);
		Vector3 target = transform.position;
		target.y = h;
		
		// Shoot ballistically to hero
		obj.rigidbody.isKinematic = false;
		Ballistics ballistics = GetComponent<Ballistics>();
		ballistics.Shoot(obj, target, jumpAngle);
		
		// Play jump sound
		AudioManager.instance.playEffect("amebaJump");
	}
	
	IEnumerator ProcessShot(GameObject enemyHit) {
		// Enable shot lock
		isShooting = true;
	
		// Make hero look at hit enemy
		transform.LookAt(enemyHit.transform.position);
		
		// Play shooting animation
		weapon.transform.FindChild("Bola_Boliche").gameObject.SetActive(true);
		GetComponent<Animator>().SetTrigger("Shoot");
		weapon.GetComponent<Animator>().SetTrigger("Shoot");
		
		yield return new WaitForSeconds(0.75f);
		
		weapon.transform.FindChild("Bola_Boliche").gameObject.SetActive(false);
		
		// Play shooting sound
		AudioManager.instance.playEffect("shot");
		
		// Disable shot lock
		isShooting = false;
		
		// Instantiate projectile
		GameObject newProjectile = (GameObject) Instantiate(projectile);
		Vector3 originalScale = newProjectile.transform.localScale;
		newProjectile.SetActive(true);
		newProjectile.transform.parent = weapon.transform;
		newProjectile.transform.localScale = originalScale;
		newProjectile.transform.position = transform.TransformPoint(transform.FindChild("ShotOrigin").localPosition);
		Physics.IgnoreCollision(newProjectile.collider, collider);
		
		// Shoot projectile
		Ballistics ballistics = GetComponent<Ballistics>();
		ballistics.Shoot(newProjectile, enemyHit.transform.position, 45f);
		
		// Stop moving
		enemyHit.tag = "";
		
		// Update stats
		enemiesKilled++;
	}
	
	// =============
	
	public void RetryOnClick() {
		SetGameState(GameState.PreGame);
		SetGameState(GameState.Running);
		StartGame();
	}
}