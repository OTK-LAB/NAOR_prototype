using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    private PlayerController playercon;

    [Header("Dagger")]
    [SerializeField] private GameObject daggerobj;
    [SerializeField] private int daggerAmount;
    [SerializeField] private float cooldownTime;

    private CooldownController daggerCooldownController;
    private ItemStack daggerStack;
  
    void Awake()
    {
        playercon = GetComponent<PlayerController>();

        daggerStack = GetComponent<ItemStack>();
        daggerStack.SetItem(daggerobj, daggerAmount);

        daggerCooldownController = GetComponent<CooldownController>();
        daggerCooldownController.SetCooldown(cooldownTime);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && !playercon.isBusy())
        {
            ThrowDagger();
        }
        //Eğer Queue'da dagger varsa ve cooldown'u bittiyse dagger'ı Queue'dan çıkarıp Stack'e koy
        if(daggerCooldownController.GetQueue().Count > 0){
            if(Time.time >= daggerCooldownController.GetDequeueTime()){
                //DequeueLastItem() fonksiyonuyla Queue'dan çıkardığın dagger'ı Stack'e koy
                daggerStack.PushToStack(daggerCooldownController.DequeueLastItem());
            }
        }
    }
    public void ThrowDagger()
    {
        //Stack'ten gir dagger çıkar ve dagger objesine ata
        GameObject dagger = daggerStack.PopFromStack();
        
        if(dagger != null)
        {
            if (GetComponent<PlayerController>().facingRight)
            {
                dagger.transform.position = firePoint.position;
                dagger.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                dagger.GetComponent<Dagger>().Initialize(Vector2.right);
                dagger.SetActive(true);
                StartCoroutine(startDaggerLifeTime());
                //Stack'ten çıkarmış olduğun dagger objesini Queue'ya yerleştir
                daggerCooldownController.EnqueueItem(dagger);
            }
            else
            {
                dagger.transform.position = firePoint.position;
                dagger.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                dagger.GetComponent<Dagger>().Initialize(Vector2.left);
                dagger.SetActive(true);
                StartCoroutine(startDaggerLifeTime());
               //Stack'ten çıkarmış olduğun dagger objesini Queue'ya yerleştir
                daggerCooldownController.EnqueueItem(dagger);
            }
            //Daggerların 3 saniye sonra sahneden çıkmasına yarayan coroutine
            IEnumerator startDaggerLifeTime()
            {
                yield return new WaitForSeconds(10f);
                dagger.SetActive(false);
            } 
        }
    }
}
