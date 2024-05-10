using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    public Collider itemCol;

	private void Awake() {
		itemCol= GetComponent<Collider>();
	}
}
