using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class PlayerController : MonoBehaviour
{
   PlayerControls controls;
   Vector2 move;
   public float speed = 10;
 
   void Awake()
   {
       controls = new PlayerControls();

       controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
       controls.Player.Move.canceled += ctx => move = Vector2.zero;
   }
 
   private void OnEnable()
   {
       controls.Player.Enable();
   }
   private void OnDisable()
   {
       controls.Player.Disable();
   }
 
   void Dash(InputValue dashValue)
   {
       Vector2 coordinates = dashValue.Get<Vector2>();
       // Use the coordinates for your dash logic
       // For example, you can log them to the console:
       Debug.Log("Dash coordinates = " + coordinates);
   }
 
   void FixedUpdate()
   {
       Vector2 movement = speed * Time.deltaTime * new Vector2(move.x, move.y);
       transform.Translate(movement, Space.World);
   }
}