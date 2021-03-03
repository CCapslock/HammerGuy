using UnityEngine;

public class ArtController : MonoBehaviour
{
	public ArtPreset[] ArtPresets;
	public SkinnedMeshRenderer[] EnemyMeshRenderers;
	public SkinnedMeshRenderer GoldEnemyMeshRenderer;
	public SkinnedMeshRenderer PlayerMeshRenderer;

	public MeshRenderer[] PlayerHammerMeshRenderers;
	public MeshRenderer[] FloorMeshRenderers;
	public MeshRenderer PlayerRingRenderer;
	public MeshRenderer BackWall;

	public ParticleSystemRenderer[] EnemyDeathParticles;
	public ParticleSystemRenderer GoldEnemyDeathParticles;

	public MeshRenderer EnemySpalshMaterial;
	public MeshRenderer GoldEnemySplashMaterial;

	private int _presetNum = 2;
	private string _presetNumPlayerPref = "PresetNum";

	private void Awake()
	{
		try
		{
			if (PlayerPrefs.GetInt(_presetNumPlayerPref) == ArtPresets.Length - 1)
			{
				_presetNum = PlayerPrefs.GetInt(_presetNumPlayerPref);
				PlayerPrefs.SetInt(_presetNumPlayerPref, 0);
			}
			else
			{
				_presetNum = PlayerPrefs.GetInt(_presetNumPlayerPref);
				PlayerPrefs.SetInt(_presetNumPlayerPref, PlayerPrefs.GetInt(_presetNumPlayerPref) + 1);
			}
		}
		catch
		{
			PlayerPrefs.SetInt(_presetNumPlayerPref, 0);
			_presetNum = PlayerPrefs.GetInt(_presetNumPlayerPref);
		}
		SetEnemyMaterial();
		SetPlayerMaterial();
		SetFloorMaterial();
	}
	private void SetFloorMaterial()
	{
		for (int i = 0; i < FloorMeshRenderers.Length; i++)
		{
			FloorMeshRenderers[i].material = ArtPresets[_presetNum].FloorMaterial;
		}
		BackWall.material = ArtPresets[_presetNum].RingMaterial;
	}
	private void SetPlayerMaterial()
	{
		Material[] tempMaterials = PlayerMeshRenderer.materials;
		for (int f = 0; f < PlayerMeshRenderer.materials.Length - 1; f++)
		{
			tempMaterials[f] = ArtPresets[_presetNum].PlayerMaterial;
		}
		PlayerMeshRenderer.materials = tempMaterials;
		for (int i = 0; i < PlayerHammerMeshRenderers.Length; i++)
		{
			PlayerHammerMeshRenderers[i].material = ArtPresets[_presetNum].PlayerMaterial;
		}
		PlayerRingRenderer.material = ArtPresets[_presetNum].RingMaterial;
	}
	private void SetEnemyMaterial()
	{
		//Set simple enemyes
		for (int i = 0; i < EnemyMeshRenderers.Length; i++)
		{
			Material[] tempMaterials = EnemyMeshRenderers[i].materials;
			for (int f = 1; f < tempMaterials.Length; f++)
			{
				tempMaterials[f] = ArtPresets[_presetNum].EnemyMaterial;
			}
			EnemyMeshRenderers[i].materials = tempMaterials;
		}
		for (int i = 0; i < EnemyDeathParticles.Length; i++)
		{
			EnemyDeathParticles[i].material = ArtPresets[_presetNum].EnemyDeathMaterial;
		}

		EnemySpalshMaterial.material = ArtPresets[_presetNum].EnemySplashMaterial;

		//Set gold enemyes
		Material[] tempMaterials2 = GoldEnemyMeshRenderer.materials;
		for (int f = 1; f < GoldEnemyMeshRenderer.materials.Length; f++)
		{
			tempMaterials2[f] = ArtPresets[0].GoldEnemyMaterial;
		}
		GoldEnemyMeshRenderer.materials = tempMaterials2;

		GoldEnemyDeathParticles.material = ArtPresets[_presetNum].GoldEnemyDeathMaterial;

		GoldEnemySplashMaterial.material = ArtPresets[_presetNum].GoldEnemySplashMaterial;
	}
}
