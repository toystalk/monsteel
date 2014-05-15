using UnityEditor;

public class AutoBuild {
	static void BuildAndroidPlayer()
	{
		string[] scenes = {};
		BuildPipeline.BuildPlayer (scenes, 
		                           "Builds/monsteel.apk", 
		                           BuildTarget.Android, 
		                           BuildOptions.None);
	}
}
