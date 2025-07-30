using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static GoodsDataTable;

/// <summary>
/// 씬의 주요 기능 제어
/// </summary>
public class SampleSceneManager : MonoBehaviour
{
    public Button gachaBtn;
    public Button popupCloseBtn;
    public Button inventoryBtn;
    public Button inventoryCloseBtn;
    public GameObject popupBackground;
    public GameObject popupPanel;
    public GameObject inventoryPanel;
    public GameObject particleParent;
    public Transform inventoryParentTr;
    public RawImage popupIcon;
    public RawImage fakePopupImage;
    public Text popupBrand;
    public Text popupName;
    public Text popupPrice;
    public Material particleMaterial;
    public GoodsItemContainer goodsItemPrefab;
    public GoodsDataTable dataTable;

    private Transform fakePopupImageTr;
    private Color originParticleColor;
    private List<GoodsItemContainer> inventoryGoodsItemList = new List<GoodsItemContainer>();

    void Start()
    {
        fakePopupImageTr = fakePopupImage.transform;
        originParticleColor = particleMaterial.color;

        // 버튼 기능 등록
        gachaBtn.onClick.AddListener(OnClickGacha);
        popupCloseBtn.onClick.AddListener(OnClickPopupClose);
        inventoryBtn.onClick.AddListener(OnClickInventory);
        inventoryCloseBtn.onClick.AddListener(OnClickInventoryClose);
    }

    void OnApplicationQuit()
    {
        // 파티클용 머테리얼 색상 초기화
        particleMaterial.color = originParticleColor;
    }

    // 뽑기 버튼 기능
    public void OnClickGacha()
    {
        // 연출 중 Input 제어
        gachaBtn.enabled = false;
        inventoryBtn.enabled = false;

        var goods = dataTable.GetRandomGoods();
        SetPopupData(goods);
        AddGoodsToInventory(goods);
        SetRarityEffect(goods.rarity);
        fakePopupImage.texture = goods.texture;

        // 팝업 UI 열기 연출
        popupPanel.SetActive(true);
        Vector3 currentPos = popupPanel.transform.localPosition;
        popupPanel.transform.DOLocalMoveY(currentPos.y + 1500, 0.2f).SetEase(Ease.InOutSine);
        popupPanel
            .transform.DOScale(Vector3.one, 0.2f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                popupCloseBtn.enabled = true;
                particleParent.SetActive(true);
                fakePopupImage.gameObject.SetActive(true);
            });

        popupBackground.SetActive(true);
    }

    // 팝업 UI 닫기 기능
    public void OnClickPopupClose()
    {
        // 연출 중 Input 제어
        popupCloseBtn.enabled = false;
        particleParent.SetActive(false);

        // 팝업 UI 닫기 연출
        Vector3 currentPos = popupPanel.transform.localPosition;
        popupPanel.transform.DOLocalMoveY(currentPos.y - 1500, 0.2f).SetEase(Ease.InOutSine);
        popupPanel
            .transform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                popupPanel.SetActive(false);
            });
        AnimateItemToInventory();

        popupBackground.SetActive(false);
    }

    // 인벤토리 열기 기능
    public void OnClickInventory()
    {
        inventoryPanel.SetActive(true);
    }

    // 인벤토리 닫기 기능
    public void OnClickInventoryClose()
    {
        inventoryPanel.SetActive(false);
    }

    // 팝업 창의 데이터를 세팅
    private void SetPopupData(Goods goods)
    {
        popupIcon.texture = dataTable.GetRarityTexture(goods.rarity);
        popupBrand.text = goods.brand;
        popupName.text = goods.name;
        popupPrice.text = goods.price.ToString("#,###") + " 원";
    }

    // 인벤토리에 아이템 추가
    private void AddGoodsToInventory(Goods goods)
    {
        var item = Instantiate(goodsItemPrefab, inventoryParentTr);
        item.SetGoodsItemDatas(goods, dataTable);
        inventoryGoodsItemList.Add(item);
    }

    // 인벤토리에 아이템이 들어가는 연출
    private void AnimateItemToInventory()
    {
        // Path 지정
        Vector3 startPos = fakePopupImageTr.position;
        Vector3 endPos = inventoryBtn.transform.position;
        Vector3 centerPos = (startPos + endPos) / 2;
        Vector3 pathPos = new Vector3(centerPos.x, startPos.y + ((startPos.y - endPos.y) / 3), 0);
        Vector3[] path = new Vector3[] { startPos, pathPos, endPos };

        // 애니메이션
        Sequence seq = DOTween.Sequence();
        seq.Join(fakePopupImageTr.DOPath(path, 0.6f, PathType.CatmullRom).SetEase(Ease.InQuad));
        seq.Join(fakePopupImageTr.DOScale(0.2f, 0.5f).SetEase(Ease.InQuad));
        seq.Join(
            fakePopupImageTr
                .DORotate(new Vector3(0, 0, 360 * 2), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.InQuad)
        );
        seq.OnComplete(() =>
        {
            // 연출 초기화
            fakePopupImageTr.gameObject.SetActive(false);
            fakePopupImageTr.localScale = Vector3.one;
            fakePopupImageTr.position = startPos;
            gachaBtn.enabled = true;
            inventoryBtn.enabled = true;
        });
    }

    // 레어도에 따른 이펙트 변경
    private void SetRarityEffect(Rarity rarity)
    {
        Color newColor = Color.white;
        string hexCode = "";

        switch (rarity)
        {
            case Rarity.S:
                hexCode = "#F0F59FFF";
                break;
            case Rarity.A:
                hexCode = "#6F82CCFF";
                break;
            case Rarity.B:
                hexCode = "#7FB080FF";
                break;
            case Rarity.C:
                hexCode = "#919191FF";
                break;
            default:
                break;
        }

        if (!ColorUtility.TryParseHtmlString(hexCode, out newColor))
        {
            Debug.Log("Hexcode가 올바르지 않습니다.");
            return;
        }

        particleMaterial.color = newColor;
    }
}
