using UnityEngine;
using System.Collections;
using FourX;

public class SelectionGrid : MonoBehaviour {

	public Terrain terrain;

	public Renderer rend;
	private LocalPlayer player;
	private TerrainData terrainData;
	private Vector3 terrainSize;
	private int heightmapWidth;
	private int heightmapHeight;
	private float[,] heightmapData;
	private Mesh selectionMesh;
	private Vector3[,] mapGrid = new Vector3[ 9, 9 ];
	private Vector3[] verts;

	private void Start () {
		player = transform.parent.GetComponent<LocalPlayer>();
		rend = GetComponent<Renderer> ();
		rend.enabled = false;
		indicatorOffset = 0.5f;
		GetTerrainData ();
		ConstructMesh ();
	}

	void Update () {
		if (player.selectedObject) {
			if (!rend.enabled) {
				Activate ();
			}

			renderAtPosition (player.selectedObject.transform.position);
		} else if (rend.enabled) {
			Deactivate ();
		}
		
	}


	public void Activate () {
		rend.enabled = true;
	}

	public void Deactivate () {
		rend.enabled = false;
	}


	public void renderAtPosition (Vector3 position) {
		GetHeightmapPosition (position);
		CalculateGrid ();
		UpdateMesh ();
	}

	//Position at which to center the heightmap.
	public Vector3 heightmapPos = new Vector3(0f,0f,0f);
	//An optional grid which will override the default calculations for it's size and shape.
	//public  bool[,] gridTemplate;

	//Size is the scale of the resulting grid.
	public float indicatorSize = ResourceManager.MapScale;
	//Offset moves the grid vertically, mostly for better visibility.
	public float indicatorOffset = 0.0f;

		public void GetHeightmapPosition(Vector3 rayHitPoint)
		{
			// find the heightmap position of that hit
			heightmapPos.x = ( rayHitPoint.x / terrainSize.x ) * ((float) heightmapWidth );
			heightmapPos.z = ( rayHitPoint.z / terrainSize.z ) * ((float) heightmapHeight );
			
			// convert to integer
			heightmapPos.x = Mathf.Round( heightmapPos.x );
			heightmapPos.z = Mathf.Round( heightmapPos.z );
			
			// clamp to heightmap dimensions to avoid errors
			heightmapPos.x = Mathf.Clamp( heightmapPos.x, 0, heightmapWidth - 1 );
			heightmapPos.z = Mathf.Clamp( heightmapPos.z, 0, heightmapHeight - 1 );
		}

	public void GetTerrainData()
	{
		if ( !terrain )
		{
			terrain = Terrain.activeTerrain;
		}
		
		terrainData = terrain.terrainData;
		
		terrainSize = terrain.terrainData.size;
		
		heightmapWidth = terrain.terrainData.heightmapWidth;
		heightmapHeight = terrain.terrainData.heightmapHeight;
		
		heightmapData = terrainData.GetHeights( 0, 0, heightmapWidth, heightmapHeight );
	}

	public void CalculateGrid()
	{
		for ( int z = -4; z < 5; z ++ )
		{
			for ( int x = -4; x < 5; x ++ )
			{
				Vector3 calcVector;
				
				calcVector.x = heightmapPos.x + ( x * indicatorSize );
				calcVector.x /= ((float) heightmapWidth );
				calcVector.x *= terrainSize.x;
				
				float calcPosX = heightmapPos.x + ( x * indicatorSize );
				calcPosX = Mathf.Clamp( calcPosX, 0, heightmapWidth - 1 );
				
				float calcPosZ = heightmapPos.z + ( z * indicatorSize );
				calcPosZ = Mathf.Clamp( calcPosZ, 0, heightmapHeight - 1 );
				
				calcVector.y = heightmapData[ (int)calcPosZ, (int)calcPosX ] * terrainSize.y; // heightmapData is Y,X ; not X,Y (reversed)
				calcVector.y += indicatorOffset; // raise slightly above terrain
				
				calcVector.z = heightmapPos.z + ( z * indicatorSize );
				calcVector.z /= ((float) heightmapHeight );
				calcVector.z *= terrainSize.z;
				
				mapGrid[ x + 4, z + 4 ] = calcVector;
			}
		}
	}

	public void ConstructMesh()
	{
		if ( !selectionMesh )
		{
			selectionMesh = new Mesh();
			MeshFilter f = GetComponent("MeshFilter") as MeshFilter;
			f.mesh = selectionMesh;
			selectionMesh.name = gameObject.name + "Mesh";
		}
		
		selectionMesh.Clear();  
		
		verts = new Vector3[9 * 9];
		Vector2[] uvs = new Vector2[9 * 9];
		int[] tris = new int[ 8 * 2 * 8 * 3];
		
		float uvStep = 1.0f / 8.0f;
		
		int index = 0;
		int triIndex = 0;
		
		for ( int z = 0; z < 9; z ++ )
		{
			for ( int x = 0; x < 9; x ++ )
			{
				verts[ index ] = new Vector3( x, 0, z );
				uvs[ index ] = new Vector2( ((float)x) * uvStep, ((float)z) * uvStep );
				
				if ( x < 8 && z < 8 )
				{
					tris[ triIndex + 0 ] = index + 0;
					tris[ triIndex + 1 ] = index + 9;
					tris[ triIndex + 2 ] = index + 1;
					
					tris[ triIndex + 3 ] = index + 1;
					tris[ triIndex + 4 ] = index + 9;
					tris[ triIndex + 5 ] = index + 10;
					
					triIndex += 6;
				}
				
				index ++;
			}
		}
		
		
		// - Build Mesh -
		selectionMesh.vertices = verts;
		selectionMesh.uv = uvs;
		selectionMesh.triangles = tris;
		
		selectionMesh.RecalculateBounds();  
		selectionMesh.RecalculateNormals();
	}

	public void UpdateMesh()
	{
		int index = 0;

		
		for ( int z = 0; z < 9; z ++ )
		{
			for ( int x = 0; x < 9; x ++ )
			{
				verts[ index ] = mapGrid[ x, z ];
				
				index ++;
			}
		}
		
		// assign to mesh
		selectionMesh.vertices = verts;
		
		selectionMesh.RecalculateBounds();
		selectionMesh.RecalculateNormals();
	}
}


