using UnityEditor;

public class AutoBuild {
	static void BuildAndroidPlayer()
	{
		string[] scenes = {};
		BuildPipeline.BuildPlayer (scenes, 
		                           "Builds/aremi.apk", 
		                           BuildTarget.Android, 
		                           BuildOptions.None);
	}
}
