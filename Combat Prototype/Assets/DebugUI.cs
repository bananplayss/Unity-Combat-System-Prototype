using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public static DebugUI Instance { get; private set; }

    [SerializeField] TextMeshProUGUI debugText;

	private void Awake() {
		Instance= this;
	}

	public void SetText(string text) {
        debugText.text = text;
    }
}
