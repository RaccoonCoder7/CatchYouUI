using UnityEngine;
using UnityEngine.UI;
using static GoodsDataTable;

/// <summary>
/// 상품 UI 컴포넌트를 캐싱하고 있는 Container
/// </summary>
public class GoodsItemContainer : MonoBehaviour
{
    public RawImage image;
    public RawImage icon;
    public Text brand;
    public Text goodsName;
    public Text price;

    // 모든 상품 UI에 데이터를 설정
    public void SetGoodsItemDatas(Goods goods, GoodsDataTable dataTable)
    {
        image.texture = goods.texture;
        icon.texture = dataTable.GetRarityTexture(goods.rarity);
        brand.text = goods.brand;
        goodsName.text = goods.name;
        price.text = goods.price.ToString("#,###") + " 원";
    }
}
