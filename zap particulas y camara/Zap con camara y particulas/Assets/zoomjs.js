 var minFov: float = 15f;
 var maxFov: float = 90f;
 var sensitivity: float = 10f;
 
 function Update () {
   var fov: float = Camera.main.fieldOfView;
   fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
   fov = Mathf.Clamp(fov, minFov, maxFov);
   Camera.main.fieldOfView = fov;
 }