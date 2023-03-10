using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class RiggingXRay : MonoBehaviour 
{
    public Transform rootNode; 		/// <summary>Root Node.</summary>
    public Transform[] childNodes; 	/// <summary>Root Node's Childs.</summary>
 
    void OnDrawGizmosSelected()
    {
        if (rootNode != null)
        {
            if(childNodes == null)
            {
                //get all joints to draw
                PopulateChildren();
            }
             
	        if( childNodes == null || childNodes.Length == 0 )
	        {
	        	foreach (Transform child in childNodes)
	            {
	                 
	                if (child == rootNode)
	                {
	                    //list includes the root, if root then larger, green cube
	                    Gizmos.color = Color.green;
	                    Gizmos.DrawCube(child.position, new Vector3(.1f, .1f, .1f));
	                }
	                else
	                {
	                    Gizmos.color = Color.blue;
	                    Gizmos.DrawLine(child.position, child.parent.position);
	                    Gizmos.DrawCube(child.position, new Vector3(.01f, .01f, .01f));
	                }
	            }
	        }
        }
     }
 
    public void PopulateChildren()
    {
        childNodes = rootNode.GetComponentsInChildren<Transform>();
    }
}