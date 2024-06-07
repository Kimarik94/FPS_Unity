using UnityEngine;
using TMPro;

public class AmmoStatus : MonoBehaviour
{
    private GunLogic gun;

    private TextMeshProUGUI ammoClipCount;
    private TextMeshProUGUI ammoTotalCount;

    private int ammoIntClipCount;
    private int ammoIntTotalCount;

    public int ammoCurrentAmount;

    private void Start()
    {
        gun = transform.root.root.GetComponentInChildren<GunLogic>();

        ammoClipCount = GameObject.Find("AmmoCount").GetComponent<TextMeshProUGUI>();
        ammoTotalCount = GameObject.Find("AmmoTotal").GetComponent<TextMeshProUGUI>();

        ammoClipCount.text = "30";
        ammoTotalCount.text = "90";
    }

    private void FixedUpdate()
    {
        ammoCurrentAmount = int.Parse(ammoClipCount.text);
    }

    public void DecreaseAmmoClipAmount()
    {
        ammoIntClipCount = int.Parse(ammoClipCount.text);
        if (ammoIntClipCount > 0)
        {
            ammoIntClipCount--;
            ammoClipCount.text = ammoIntClipCount.ToString();
        }

        if (ammoIntClipCount == 0)
        {
            gun.ammoClipDry = true;
        }
    }

    public void Reload()
    {
        ammoIntClipCount = int.Parse(ammoClipCount.text);
        ammoIntTotalCount = int.Parse(ammoTotalCount.text);

        if(ammoIntTotalCount > 0)
        {
            int ammoNeedForFullClip = 30 - ammoIntClipCount;

            if(ammoIntTotalCount - ammoNeedForFullClip > 0)
            {
                ammoIntTotalCount -= ammoNeedForFullClip;
                ammoIntClipCount = 30;
            }
            else
            {
                ammoIntClipCount += ammoIntTotalCount;
                ammoIntTotalCount = 0;
            }
            
            ammoTotalCount.text = ammoIntTotalCount.ToString();
            ammoClipCount.text = ammoIntClipCount.ToString();
            gun.ammoClipDry = false;
        }

        if (ammoIntTotalCount == 0) gun.ammoTotalDry = true;
    }
}
