using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using VoidlessUtilities;

/// \TODO Give most of the credit to Liorta's Code Template Asset on repository: https://bitbucket.org/liortal/code-templates
namespace VoidlessUtilities
{
public class DoCreateCodeFile : EndNameEditAction
{
	/// \TODO Check EndNameEditAction's API.
	public override void Action(int _instanceId, string _pathName, string _resourceFile)
	{
		Object obj = CustomScriptsCreator.CreateScript(_pathName, _resourceFile);
		ProjectWindowUtil.ShowCreatedAsset(obj);
	}
}

public class CustomScriptsCreator
{
	private const string C_SHARP_SCRIPT_ICON = "cs Script Icon"; 													/// <summary>C#'s icon name.</summary>
	private const int MENU_ITEM_PRIORITY = 70; 																		/// <summary>Menu Item's order priority index.</summary>

#region ReplaceTags:
	private const string REPLACE_SCRIPTNAME_TAG = "#SCRIPTNAME#"; 													/// <summary>Script name's replace tag.</summary>
	private const string REPLACE_UPPERCASE_SCRIPTNAME_TAG = "#UPPERCASE_SCRIPTNAME#"; 								/// <summary>Script name's replace tag [for Upper case].</summary>
	private const string REPLACE_NOTRIM_TAG = "#NOTRIM#"; 															/// <summary>Empty space's replace tag.</summary>
	private const string REPLACE_TARGETNAME_TAG = "#TARGETNAME#"; 													/// <summary>Target name's replace tag [for Inspector's Template].</summary>
	private const string REPLACE_TARGETVARIABLE_TAG = "#TARGETVARIABLE#"; 											/// <summary>Target variable's replace tag [for Inspector's Template].</summary>
	private const string REPLACE_INSPECTOR_TAG = "Inspector"; 														/// <summary>Inspector's label replace tag [for Inspector's Template].</summary>
#endregion	

#region InitialNames:
	private const string MONOBEHAVIOUR_INITIAL_NAME = "NewBehavior.cs"; 											/// <summary>MonoBehaviour's dummy name before setting.</summary>
	private const string INSPECTOR_INITIAL_NAME = "NewInspector.cs"; 												/// <summary>Inspector's dummy name before setting.</summary>
	private const string SCRIPTABLEOBJECT_INITIAL_NAME = "NewScriptableObject.cs"; 									/// <summary>ScriptableObject's dummy name before setting.</summary>
	private const string C_SHARP_INITIAL_NAME = "NewClass.cs"; 														/// <summary>C# class' dummy name before setting.</summary>
	private const string INTERFACE_INITIAL_NAME = "NewInterface.cs"; 												/// <summary>Interface's dummy name before setting.</summary>
	private const string STRUCT_INITIAL_NAME = "NewStruct.cs"; 														/// <summary>Struct's dummy name before setting.</summary>
	private const string EDITORWINDOW_INITIAL_NAME = "NewEditorWindow.cs"; 											/// <summary>EditorWindow's dummy name before settings.</summary>
#endregion

#region MenuItemPaths:
	private const string MENU_ITEM_PATH = "Assets/Create Custom Script"; 											/// <summary>Custom Scripts default Path.</summary>
	private const string MENU_ITEM_C_SHARP_PATH = "/C#"; 															/// <summary>Menu Item's C# path index.</summary>
	private const string MENU_ITEM_CUSTOM_MONOBEHAVIOUR = "/MonoBehaviour"; 										/// <summary>Menu Item's MonoBehaviour index.</summary>
	private const string MENU_ITEM_CUSTOM_INSPECTOR = "/Inspector"; 												/// <summary>Menu Item's Inspector index.</summary>
	private const string MENU_ITEM_CUSTOM_SCRIPTABLEOBJECT = "/Scriptable Object"; 									/// <summary>Menu Item's ScriptableObject index.</summary>
	private const string MENU_ITEM_CUSTOM_EDITORWINDOW = "/Editor Window"; 											/// <summary>Menu Item's EditorWindow index.</summary>
	private const string MENU_ITEM_CUSTOM_C_SHARP = "/Class"; 														/// <summary>Menu Item's C# Class index.</summary>
	private const string MENU_ITEM_CUSTOM_INTERFACE = "/Interface"; 												/// <summary>Menu Item's Interface index.</summary>
	private const string MENU_ITEM_CUSTOM_STRUCT = "/Struct"; 														/// <summary>Manu Item's Struct index.</summary>
#endregion

#region TemplatePaths:
	private const string TEMPLATES_FOLDER_PATH = "Assets/Custom Script Templates"; 									/// <summary>Custom Templates's root path.</summary>
	private const string TEMPLATE_MONOBEHAVIOUR_PATH = "/Template_MonoBehaviour.cs.txt"; 							/// <summary>MonoBehaviour's Template path.</summary>
	private const string TEMPLATE_INSPECTOR_PATH = "/Template_Inspector.cs.txt"; 									/// <summary>Inspector's Template path.</summary>
	private const string TEMPLATE_SCRIPTABLEOBJECT_PATH = "/Template_ScriptableObject.cs.txt"; 						/// <summary>ScriptableObject's Template path.</summary>
	private const string TEMPLATE_EDITORWINDOW_PATH = "/Template_EditorWindow.cs.txt"; 								/// <summary>EditorWindow's Template path.</summary>
	private const string TEMPLATE_C_SHARP_PATH = "/Template_C#_Class.cs.txt"; 										/// <summary>C# Class's Template path.</summary>
	private const string TEMPLATE_INTERFACE_PATH = "/Template_C#_Interface.cs.txt"; 								/// <summary>Interface's Template path.</summary>
	private const string TEMPLATE_STRUCT_PATH = "/Template_C#_Struct.cs.txt"; 										/// <summary>Struct's Template path.</summary>
#endregion

#region Messages:
	private const string WINDOW_ERROR_LABEL = "Script Template Not Found!"; 										/// <summary>Window Error Label Message.</summary>
	private const string WINDOW_ERROR_CONTENT = "There is no Template at path: \n\n"; 								/// <summary>Window Error Content Message.</summary>
	private const string WINDOW_OK_ANSWER = "Ok"; 																	/// <summary>Window Error Accept Button Label.</summary>
#endregion

	private static Texture2D scriptIcon = (EditorGUIUtility.IconContent (C_SHARP_SCRIPT_ICON).image as Texture2D); 	/// <summary>MonoBehaviour's Script icon.</summary>

#region ScriptCreationMethods:
	/// <summary>MonoBehaviour Script's creation procedures.</summary>
	[MenuItem(MENU_ITEM_PATH + MENU_ITEM_C_SHARP_PATH + MENU_ITEM_CUSTOM_MONOBEHAVIOUR, false, MENU_ITEM_PRIORITY)]
	public static void CreateMonoBehaviourScript()
	{
		CreateFromTemplate(MONOBEHAVIOUR_INITIAL_NAME, TEMPLATES_FOLDER_PATH + TEMPLATE_MONOBEHAVIOUR_PATH);
	}

	/// <summary>Inspector Script's creation procedures.</summary>
	[MenuItem(MENU_ITEM_PATH + MENU_ITEM_C_SHARP_PATH + MENU_ITEM_CUSTOM_INSPECTOR, false, MENU_ITEM_PRIORITY)]
	public static void CreateInspectorScript()
	{
		CreateFromTemplate(INSPECTOR_INITIAL_NAME, TEMPLATES_FOLDER_PATH + TEMPLATE_INSPECTOR_PATH);
	}

	/// <summary>ScriptableObject Script's creation procedures.</summary>
	[MenuItem(MENU_ITEM_PATH + MENU_ITEM_C_SHARP_PATH + MENU_ITEM_CUSTOM_SCRIPTABLEOBJECT, false, MENU_ITEM_PRIORITY)]
	public static void CreateScriptableObjectScript()
	{
		CreateFromTemplate(SCRIPTABLEOBJECT_INITIAL_NAME, TEMPLATES_FOLDER_PATH + TEMPLATE_SCRIPTABLEOBJECT_PATH);
	}

	/// <summary>EditorWindow Script's creation procedures.</summary>
	[MenuItem(MENU_ITEM_PATH + MENU_ITEM_C_SHARP_PATH + MENU_ITEM_CUSTOM_EDITORWINDOW, false, MENU_ITEM_PRIORITY)]
	public static void CreateEditorWindowScript()
	{
		CreateFromTemplate(EDITORWINDOW_INITIAL_NAME, TEMPLATES_FOLDER_PATH + TEMPLATE_EDITORWINDOW_PATH);
	}

	/// <summary>C# Class's Script's creation procedures.</summary>
	[MenuItem(MENU_ITEM_PATH + MENU_ITEM_C_SHARP_PATH + MENU_ITEM_CUSTOM_C_SHARP, false, MENU_ITEM_PRIORITY)]
	public static void CreateCSharpClassScript()
	{
		CreateFromTemplate(C_SHARP_INITIAL_NAME, TEMPLATES_FOLDER_PATH + TEMPLATE_C_SHARP_PATH);
	}

	/// <summary>Struct's Script's creation procedures.</summary>
	[MenuItem(MENU_ITEM_PATH + MENU_ITEM_C_SHARP_PATH + MENU_ITEM_CUSTOM_STRUCT, false, MENU_ITEM_PRIORITY)]
	public static void CreateStructClassScript()
	{
		CreateFromTemplate(STRUCT_INITIAL_NAME, TEMPLATES_FOLDER_PATH + TEMPLATE_STRUCT_PATH);
	}

	/// <summary>Interface's Script's creation procedures.</summary>
	[MenuItem(MENU_ITEM_PATH + MENU_ITEM_C_SHARP_PATH + MENU_ITEM_CUSTOM_INTERFACE, false, MENU_ITEM_PRIORITY)]
	public static void CreateInterfaceScript()
	{
		CreateFromTemplate(INTERFACE_INITIAL_NAME, TEMPLATES_FOLDER_PATH + TEMPLATE_INTERFACE_PATH);
	}
#endregion
		
	/// <summary>Creates Script on Path.</summary>
	/// <param name="_filePath">Path to import the Script.</param>
	/// <param name="_templatePath">Template's Path.</param>
	/// <returns>Created Asset.</returns>
	internal static UnityEngine.Object CreateScript(string _filePath, string _templatePath)
	{
		string newFilePath = Path.GetFullPath(_filePath);
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_filePath);
		string className = NormalizeClassName(fileNameWithoutExtension);
		string templateText = string.Empty;

		if (File.Exists(_templatePath))
		{
			UTF8Encoding encoding = new UTF8Encoding(true, false);

			/// Read the file:
			StreamReader reader = new StreamReader(_templatePath);
			templateText = reader.ReadToEnd();
			reader.Close();
			templateText = templateText.Replace(REPLACE_SCRIPTNAME_TAG, className);
			templateText = templateText.Replace(REPLACE_NOTRIM_TAG, string.Empty);

			switch(_templatePath)
			{
				case (TEMPLATES_FOLDER_PATH + TEMPLATE_INSPECTOR_PATH): //If the path given is the same of the Inspector's Template, make its respective procedures.
				string inspectorTargetName = className.Replace(REPLACE_INSPECTOR_TAG, string.Empty);
				templateText = templateText.Replace(REPLACE_TARGETNAME_TAG, inspectorTargetName);
				templateText = templateText.Replace(REPLACE_TARGETVARIABLE_TAG, inspectorTargetName.ToCamelCase());
				break;

				case (TEMPLATES_FOLDER_PATH + TEMPLATE_EDITORWINDOW_PATH): //If the path given is the same of the EditorWindow's Template, make its respective procedures.
				templateText = templateText.Replace(REPLACE_TARGETVARIABLE_TAG, className.ToCamelCase());
				templateText = templateText.Replace(REPLACE_UPPERCASE_SCRIPTNAME_TAG, className.ToUpper());
				break;

				default:
				break;
			}
			
			/// Write the file:
			StreamWriter writer = new StreamWriter(newFilePath, false, encoding);
			writer.Write(templateText);
			writer.Close();
			
			AssetDatabase.ImportAsset(_filePath);	
			return AssetDatabase.LoadAssetAtPath(_filePath, typeof(Object));
		}
		else
		{
			EditorUtility.DisplayDialog
        	(
	            WINDOW_ERROR_LABEL,
	            WINDOW_ERROR_CONTENT + _templatePath,
	            WINDOW_OK_ANSWER
	        );

			return null;
		}
	}

	/// <summary>Normalizes class's name by replacing unnecessary characters.</summary>
	/// <param name="_fileName">File's name.</param>
	/// <returns>Normalized class's name.</returns>
	private static string NormalizeClassName(string _fileName)
	{
		return _fileName.Replace(" ", string.Empty);
	}

	/// <summary>Creates a new code file from a template file.</summary>
	/// <param name="_initialName">The initial name to give the file in the UI</param>
	/// <param name="_templatePath">The full path of the template file to use</param>
	public static void CreateFromTemplate(string _initialName, string _templatePath)
	{
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists
		(
			0,
			ScriptableObject.CreateInstance<DoCreateCodeFile>(),
			_initialName,
			scriptIcon,
			_templatePath
		);
	}
}
}



/// DEPRECATED:
/*[MenuItem(CONTEXT_MENU_PATH + CONTEXT_MENU_CUSTOM_C_SHARP, false, MENU_ITEM_PRIORITY)]
public static void Create(MenuCommand _cmd)
{
	Object selection = Selection.activeObject;

	if (selection != null)
	{
		string path = AssetDatabase.GetAssetPath(selection);

        if (File.Exists(path))
        {
        	path = Path.GetDirectoryName(path);
    	}
        if (string.IsNullOrEmpty(path))
        {
        	path = "Assets/";
        }
 
 		//string newName = EditorUtility.SaveFilePanelInProject("Save Your Prefab", "NewScript.cs", "cs", "Please select file name to save prefab to:");
        //File.Copy((CUSTOM_TEMPLATE_FOLDER_PATH + "/" + CUSTOM_C_SHARP_TEMPLATE_PATH), Path.Combine(path, "NewScript.cs"));
        if(AssetDatabase.CopyAsset((CUSTOM_TEMPLATE_FOLDER_PATH + "/" + CUSTOM_C_SHARP_TEMPLATE_PATH), Path.Combine(path, "NewName.cs")))
        {
        	//AssetDatabase.RenameAsset(Path.Combine(path, "NewName.cs"), "KillMePlease.cs");

        	AssetDatabase.Refresh();
        }
        else 
        {
        	EditorUtility.DisplayDialog
        	(
	            PATH_NOT_FOUND_MESSAGE,
	            WINDOW_ERROR_MESSAGE + ((CUSTOM_TEMPLATE_FOLDER_PATH + "/" + CUSTOM_C_SHARP_TEMPLATE_PATH)),
	            WINDOW_OK_ANSWER
	        );
        }
	}
    
}*/