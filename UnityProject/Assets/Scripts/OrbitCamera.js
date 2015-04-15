var target : Transform;
var targetHeight = 2.0;
var distance = 2.8;
var maxDistance = 10;
var minDistance = 0.5;
var xSpeed = 250.0;
var ySpeed = 1000.0;
var yMinLimit = -40;
var yMaxLimit = 80;
var zoomRate = 20;
var rotationDampening = 3.0;
private var x = 0.0;
private var y = 0.0;
var isTalking:boolean = false;
 
@script AddComponentMenu("Camera-Control/WoW Camera")
 
function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
 
   // Make the rigid body not change rotation
      if (GetComponent.<Rigidbody>())
      GetComponent.<Rigidbody>().freezeRotation = true;
}
 
function LateUpdate () {
   if(!target)
      return;
   
   // If either mouse buttons are down, let them govern camera position
   if (Input.GetMouseButton(0) || (Input.GetMouseButton(1))){
   x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
   y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
   
   
   // otherwise, ease behind the target if any of the directional keys are pressed
   } else if(Input.GetAxis("Vertical") || Input.GetAxis("Horizontal")) {
      var targetRotationAngle = target.eulerAngles.y;
      var currentRotationAngle = transform.eulerAngles.y;
      x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
    }
     
 
   distance -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
   distance = Mathf.Clamp(distance, minDistance, maxDistance);
   
   y = ClampAngle(y, yMinLimit, yMaxLimit);
   
  // ROTATE CAMERA:
   var rotation:Quaternion = Quaternion.Euler(y, x, 0);
   transform.rotation = rotation;
   
   // POSITION CAMERA:
   var position = target.position - (rotation * Vector3.forward * distance + Vector3(0,-targetHeight,0));
   transform.position = position;
   
    // IS VIEW BLOCKED?
    var hit : RaycastHit;
    var trueTargetPosition : Vector3 = target.transform.position - Vector3(0,-targetHeight,0);
    // Cast the line to check:
    if (Physics.Linecast (trueTargetPosition, transform.position, hit)) {  
        // If so, shorten distance so camera is in front of object:
        var tempDistance = Vector3.Distance (trueTargetPosition, hit.point) - 0.28;
        // Finally, rePOSITION the CAMERA:
        position = target.position - (rotation * Vector3.forward * tempDistance + Vector3(0,-targetHeight,0));
        transform.position = position;
    }
}
 
static function ClampAngle (angle : float, min : float, max : float) {
   if (angle < -360)
      angle += 360;
   if (angle > 360)
      angle -= 360;
   return Mathf.Clamp (angle, min, max);
   
}