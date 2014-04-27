using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour {

	public GameObject pieceCubePrefab;
	public int difficulty = 1;
	public float minInstantiationRadius = 1.0f;
	public float maxInstantiationRadius = 2.0f;

	private GamePiece[] gamePieces;

	private int getNumGamePieces() {
		return difficulty + 1;
	}

	void Start () {
		int numGamePieces = getNumGamePieces();
		gamePieces = new GamePiece[numGamePieces];
		for (int i = 0; i < numGamePieces; i++) {
			Vector2 pos2d = Random.insideUnitCircle;
			float instantiationRadius = Random.Range(minInstantiationRadius, maxInstantiationRadius);
			Vector3 pos = new Vector3(pos2d.x, 0, pos2d.y) * instantiationRadius;
			Vector2 dir2d = Random.insideUnitCircle;
			Vector3 toDir = new Vector3(dir2d.x, 0, dir2d.y);
			Vector3 fromDir = new Vector3(1, 0, 0);
			Quaternion rot = Quaternion.FromToRotation(fromDir, toDir);
			GameObject gamePieceObj = Instantiate(pieceCubePrefab, pos, rot) as GameObject;
			GamePiece gamePiece = gamePieceObj.GetComponent<GamePiece>();
			gamePiece.gamePieceId = i;
			gamePieces[i] = gamePiece;
		}
	}

	void Update () {
	
	}
}
