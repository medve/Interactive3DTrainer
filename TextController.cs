using UnityEngine;
using System.Collections;

public interface TextController {

	void SetText (string text);

	void SetTextPosition (Vector3 position);

	void SetTextStyle (FontStyle fontStyle);

	void HideText ();

	void ShowText ();

}
