using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound; //sound that we will play when picking up a new checkpoint
    private Transform currentCheckpoint; //we will store our last checkpoint here
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        //check is check point available
        if (currentCheckpoint == null)
        {
            // show game over screen
            uiManager.GameOver();

            return; //don't execute the rest of this function
        }

        transform.position = currentCheckpoint.position; // move player to checkpoint position
        playerHealth.Respawn(); //restore player health and reset animation

        //move camera to checkpoint room
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    //activate checkpoints
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; //store the checkpoint that we activated as the current one
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; //deactivate checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("appear"); //trigger checkpoint animation
        }
    }
}
