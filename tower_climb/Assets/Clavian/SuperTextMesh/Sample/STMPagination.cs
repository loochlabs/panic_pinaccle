using UnityEngine;
using System.Collections;

public class STMPagination : MonoBehaviour {
	public SuperTextMesh originalText;
	public SuperTextMesh overflowText;
	
	public void OverflowLeftovers(){
		overflowText.text = originalText.leftoverText.TrimStart();
		//if there's not text, Rebuild() doesn't get called anyway
		//use TrimStart() to remove any spaces that might have carried over.
	}
}
