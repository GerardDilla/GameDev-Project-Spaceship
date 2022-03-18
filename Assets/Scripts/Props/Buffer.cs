using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buffer : MonoBehaviour
{
    // public UnityEvent buffEvent;
    public enum buffchoices { Heal, AttackSpeed, Shield };
    public buffchoices buffs;
    public float amount;
    public float duration;
    public GameObject acquiredBuffEffect;
    private ObjectPooler objectPooler;

    private void OnEnable()
    {
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
    }
    private void LateUpdate()
    {

        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(40, 255, 0, 100), Mathf.PingPong(Time.time, 1));

    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (buffs == buffchoices.Heal)
            {
                other.gameObject.GetComponent<ShipControl>().Repair(amount);
            }
            if (buffs == buffchoices.AttackSpeed)
            {
                other.gameObject.GetComponent<ShipControl>().AttackSpeedBuff(amount, duration);
            }
            if (buffs == buffchoices.Shield)
            {
                other.gameObject.GetComponent<ShipControl>().ShielfBuff(amount, duration);
            }
            AcquiredBuff();
        }

    }
    void AcquiredBuff()
    {

        GameObject objectDestroyParticle = objectPooler.GetPooledObject(acquiredBuffEffect);
        // GameObject objectDestroyParticle = Instantiate(acquiredBuffEffect);
        objectDestroyParticle.SetActive(true);
        objectDestroyParticle.transform.position = transform.position;
        objectDestroyParticle.GetComponent<ParticleSystem>().Play();
        gameObject.SetActive(false);
    }
}
