using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CollisionAbsorbing : MonoBehaviour
{
    private SizeManager sizeManager;
    private GameObject UIControllerObject;
    private XPScalingBehaviour XPScaling;
    public float GrownAmount;
    public GameObject WhiteBloodCellPrefab;
    void Start()
    {
        sizeManager = gameObject.GetComponent<SizeManager>();
        UIControllerObject = GameObject.FindWithTag("UIController");
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (gameObject.CompareTag("WhiteCell") && other.gameObject.CompareTag("Player"))
        {
            WhiteBloodCellDrain(other.GetComponent<SizeManager>(), 0.5f, 0.2f);
        }

        if (gameObject.CompareTag("Player") && other.gameObject.CompareTag("RedCell"))
        {
            Debug.Log("Player Draid");
            DrainRedCell(other.GetComponent<SizeManager>(), 0.8f, 0.5f);
        }
    }

    public void DrainRedCell(SizeManager other, float drainWeight, float growthWeight)
    {
        float sizeRatio = sizeManager.size / Mathf.Max(other.size, 0.1f);
        float baseDrain = 0.01f;

        // Apply a logarithmic scale to smooths extreme differences
        float scaledSizeRatio = Mathf.Log(sizeRatio + 1.0f); 

        // Drain scales with the cell's size advantage
        float drainAmount = (drainWeight * scaledSizeRatio + baseDrain) * Time.deltaTime;
        drainAmount = Mathf.Min(drainAmount, other.size); // Prevent over-draining

        // Increase Player size based on drained amount (scaling effect)
        float sizeScaling = other.size / (sizeManager.size + other.size); // Small cells give less
        float growthFactor = Mathf.Clamp01(scaledSizeRatio * growthWeight * sizeScaling); // Growth scales with size difference but isn't extreme
        float actualGrowth = drainAmount * growthFactor;

        sizeManager.IncreaseSpriteSize(actualGrowth);
        other.DecreaseSpriteSize(drainAmount);

        // Destroy cell if too small
        float smallestSize = sizeManager.size * 0.1f;
        if (other.size < smallestSize)
        {
            UIControllerObject.GetComponent<XPScalingBehaviour>().AddRedBloodCell();

            // Apply velocity towards the red cell's last position
            Vector2 redCellPosition = other.transform.position; 
            gameObject.GetComponent<PlayerController>().PullTowards(redCellPosition);
            Destroy(other.gameObject);
        }
    }

    public void WhiteBloodCellDrain(SizeManager player, float drainWeight, float growthWeight)
    {
        float sizeRatio = sizeManager.size / player.size;
        float baseDrain = 0.01f;

        // Apply a logarithmic scale to smooths extreme differences
        float scaledSizeRatio = Mathf.Log(sizeRatio + 1.0f);

        // Drain scales with the cell's size advantage
        float drainAmount = (drainWeight * scaledSizeRatio + baseDrain) * Time.deltaTime;
        drainAmount = Mathf.Min(drainAmount, player.size); // Prevent over-draining

        // Increase White Blood Cell size based on drained amount (scaling effect)
        float growthFactor = Mathf.Clamp01(scaledSizeRatio * growthWeight); // Growth scales with size difference but isn't extreme
        float actualGrowth = drainAmount * growthFactor;

        if (GrownAmount > 0.07)
        {
            sizeManager.IncreaseSpriteSize(-GrownAmount);
            GrownAmount = 0;
            Vector3 spawnPosition = new Vector3(15f, 0f, 0f);
            Instantiate(WhiteBloodCellPrefab, spawnPosition,Quaternion.identity);
        }
        else
        {
            sizeManager.IncreaseSpriteSize(actualGrowth);
        }
        

        GrownAmount += actualGrowth;
        player.DecreaseSpriteSize(drainAmount);

        // Destroy player if too small
        if (player.size < 0.1f)
        {
            Debug.Log(player.size);
            player.GetComponent<PlayerState>().isAlive = false;
            player.GetComponent<PlayerState>().RestartGame();
            player.gameObject.GetComponent<PlayerController>().isActive = false;
            
        }
    }
}
