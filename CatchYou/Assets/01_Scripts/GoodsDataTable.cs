using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 상품에 대한 정보를 저장하는 SO
/// </summary>
[CreateAssetMenu(fileName = "GoodsDataTable", menuName = "SO/GoodsDataTable", order = 0)]
public class GoodsDataTable : ScriptableObject
{
    public List<Goods> goodsList = new List<Goods>();
    public List<RarityData> rarityDataList = new List<RarityData>();

    [System.Serializable]
    public class Goods
    {
        public string goodsKey;
        public string brand;
        public string name;
        public Rarity rarity;
        public int price;
        public Texture texture;
    }

    [System.Serializable]
    public class RarityData
    {
        public Rarity rarity;
        public Texture icon;
    }

    [System.Serializable]
    public enum Rarity
    {
        S,
        A,
        B,
        C,
    }

    /// <summary>
    /// goodsList 내의 랜덤한 상품을 반환
    /// </summary>
    /// <returns>Goods</returns>
    public Goods GetRandomGoods()
    {
        int randomIndex = Random.Range(0, goodsList.Count);
        return goodsList[randomIndex];
    }

    /// <summary>
    /// rarity에 맞는 아이콘 텍스쳐 반환
    /// </summary>
    /// <returns>Texture</returns>
    public Texture GetRarityTexture(Rarity rarity)
    {
        var rarityData = rarityDataList.Find(x => x.rarity.Equals(rarity));
        if (rarityData == null || rarityData.icon == null)
        {
            Debug.Log("rarity 데이터에 맞는 텍스쳐가 존재하지 않습니다");
            return null;
        }

        return rarityData.icon;
    }
}
