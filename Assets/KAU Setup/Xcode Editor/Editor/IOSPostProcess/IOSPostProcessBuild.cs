using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS || UNITY_IPHONE
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
using System.Xml;

namespace Services.IOS.Editor
{
    public static class XcodeModifier
    {

        [PostProcessBuild]
        public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {

            if (buildTarget == BuildTarget.iOS)
			{
				#if UNITY_IOS || UNITY_IPHONE
	
            	string plistPath = pathToBuiltProject + "/Info.plist";
				
            	PlistDocument plist = new PlistDocument();
            	plist.ReadFromString(File.ReadAllText(plistPath));
       	
            	// Get root
            	PlistElementDict rootDict = plist.root;
       	
            	// Change value of CFBundleVersion in Xcode plist
            	var changeInfoField = "ITSAppUsesNonExemptEncryption";
            	rootDict.SetBoolean(changeInfoField,"false"); // set boolean
	
            	changeInfoField = "NSLocationWhenInUseUsageDescription";
            	rootDict.SetString(changeInfoField,"Is Not In Use");
	
	        	 // PlistElementDict rootDict = plist.root;
				PlistElementArray bgModes = rootDict.CreateArray("CFBundleLocalizations");
				bgModes.AddString("en");
				bgModes.AddString("fr");
				bgModes.AddString("es");
				bgModes.AddString("pt");
				bgModes.AddString("it");
				bgModes.AddString("de");
				bgModes.AddString("zh-Hans");
				bgModes.AddString("zh-Hant");
				bgModes.AddString("nl");
				bgModes.AddString("ja");
				bgModes.AddString("vi");
				bgModes.AddString("ko");
				bgModes.AddString("ru");
				bgModes.AddString("uk");
				bgModes.AddString("sv");
				bgModes.AddString("da");
				bgModes.AddString("nb");
				bgModes.AddString("fi");
				bgModes.AddString("tr");
				bgModes.AddString("el");
				bgModes.AddString("id");
				bgModes.AddString("ms");
				bgModes.AddString("th");
	        	    
	
            	// Write to file
            	File.WriteAllText(plistPath, plist.WriteToString());
            	// add Entitlements:
	
            	// Get Xcode project (.pbxproj)
            	string pbxPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
				
            	var capManager = new ProjectCapabilityManager(pbxPath, "ios.entitlements", "Unity-iPhone");
            	capManager.AddInAppPurchase();
	        	capManager.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications); //todo: Problem is: is not setting up remote notification flag
	        	//capManager.AddGameCenter(); - not working TODO: Check where to set it up
            	
            	bool developmentMode = false; // Development mode should be used while testing your application outside of the App Store.
					
            	capManager.AddPushNotifications(developmentMode); //todo : Need to add the Certificate problem missing some value in plist. Need to investigate
            	capManager.WriteToFile();
	
	        	
				#endif
			}
        }
    }

}