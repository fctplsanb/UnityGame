using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;

public static class PostProcessBuildTrigger {
#if UNITY_IOS
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget target, string path) {
		string plistpath = path;
		updateInfoPlist(plistpath,"NSLocationAlwaysUsageDescription",PlayerPrefs.GetString("NSLocationUsageDescription","DUMMY, bitte in Scene ändern"));

	}

	public static void updateInfoPlist(string path, string key, string value) {
		string plistfile = path + "/Info.plist";
		string[] lines = System.IO.File.ReadAllLines(plistfile);
		string newcontent = "";
		for (int i = 0; i < lines.Length; i++) {
			if (i < lines.Length - 2)  {
				if (!lines[i].Contains(key)) {
					newcontent += lines[i];
					newcontent += "\n";
				}
			}
		}
		newcontent += "\t<key>"+key+"</key>\n<string>"+value+"</string>\n"+lines[lines.Length-2]+"\n"+lines[lines.Length-1];
		FileStream filestr = new FileStream(plistfile,FileMode.Create);
		filestr.Close();
		StreamWriter plistWriter = new StreamWriter(plistfile);
		plistWriter.Write(newcontent);
		plistWriter.Close();
	}
#endif
}
