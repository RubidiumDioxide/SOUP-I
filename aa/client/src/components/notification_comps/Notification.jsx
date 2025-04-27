import {React, useState, useEffect} from 'react'; 
import {Link} from 'react-router-dom'; 

// deconstructed props
export default function Notification({notification, notification:{id, projectId, senderId, receiverId, role, type}, onAction}) {
  const [project, setProject] = useState({}); 
  const [sender, setSender] = useState({});  
  const [refreshCond, setRefreshCond] = useState([true]);

  //load project 
  useEffect(() =>{
    fetch(`/api/Projects/${projectId}`, {
      method: "GET"})
      .then(response => response.json())
      .then(p => setProject(p));  

    fetch(`/api/Users/${senderId}`, {
      method: "GET"})
      .then(response => response.json())
      .then(u => setSender(u)); 
  }, []); 

  function Accept(){
    var team; 
    
    if(type == "invite"){
      team = {
        id : 0, 
        userId : receiverId, 
        userName: "",  // not needed for POST
        projectId : projectId,
        projectName : "", //not needed for POST 
        role : role,  
        level : 0  
      } 
    }
    else if(type == "request"){
      team = {
        id : 0, 
        userId : senderId, 
        userName: "",  // not needed for POST
        projectId : projectId,
        projectName : "", //not needed for POST 
        role : role, 
        level : 0 
      }  
    }

    fetch(`/api/Teams`, {
      method : "POST", 
      headers : {
        "content-Type" : "application/json"
      }, 
      body: JSON.stringify(team)
    })
      .then(response => response.ok?
      // DELETE request to delete invite entry  
        fetch(`/api/Notifications/${id}`, {
          method : "DELETE", 
          headers: {
            "Content-Type" : "application/json"  
          }
        })  
          .then(onAction)
        :
        null
      ); 
  } 

  function Decline(){
    // DELETE request to delete invite entry  
    fetch(`/api/Notifications/${id}`, {
      method : "DELETE", 
      headers: {
        "Content-Type" : "application/json"  
      }
    })  
      .then(onAction); 
  } 

  return (
    {project}? 
    // if project loaded
    <div>
      <p>
        {type=="invite"?
        <>
          Вас пригласили в команду проекта
          <Link to={`/project/${projectId}`}> {project.name} </Link>
          на роль {role}!
        </>
        :
        null}
        {type=="request"?
        <>
          <Link to={`/user/${senderId}`}> {sender.name} </Link>  
          хочет участвовать в вашем проекте 
          <Link to={`/project/${projectId}`}> {project.name} </Link>
          как {role}. 
        </>
        :
        null}

      </p>

      <p>
        <button class='rounded-button' onClick={Accept}>
          Accept
        </button>
        <button class='rounded-button' onClick={Decline}>
          Decline
        </button>
      </p>
    </div>
    :
    // if not 
    <p>{"Sorry, I messed up the loading :( text me @rubidiumoxide"}</p>  
  )
} 
