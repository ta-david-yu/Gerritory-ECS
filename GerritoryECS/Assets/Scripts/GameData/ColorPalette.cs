using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "GameData/ColorPalette")]
public sealed class ColorPalette : ScriptableObject
{
	[System.Serializable]
	private class ColorSet
	{
		public Color PlayerBodyColor;
		public Color TileBodyColor;
		public Color FogColor;
	}

	[SerializeField]
	private List<ColorSet> m_ColorSets = new List<ColorSet>();

	public int NumberOfColorSets => m_ColorSets.Count;

	public Color GetPlayerBodyColorForTeam(int teamId)
	{
		return m_ColorSets[teamId].PlayerBodyColor;
	}

	public Color GetTileBodyColorForTeam(int teamId)
	{
		return m_ColorSets[teamId].TileBodyColor;
	}

	public Color GetFogColorForTeam(int colorId)
	{
		return m_ColorSets[colorId].FogColor;
	}
}
