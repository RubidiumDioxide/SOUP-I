import React, { useEffect, useState } from "react"; 
import Edit from "./Edit"

export default function IndUser() { 
  const id = (window.location.pathname == "/app/user")? (sessionStorage.getItem("savedUserID")) : (window.location.pathname.split("/")[3]); 

  const uri = `/api/users/${id}`; 
  const [user, setUser] = useState(null);   
  const [refreshCond, setRefreshCond] = useState([true]);
  const [isEditing, setIsEditing] = useState(false); 

  useEffect(()=>{
    fetch(uri)
      .then(response => response.json())
      .then(u => setUser(u))
      .then(setRefreshCond([false]))
  }, refreshCond) 

  function refresh(){
    setRefreshCond([true]);
  }

  function changeEditState(){
    setIsEditing(!isEditing); 
  }

  return (
      (user)?
        //if user is loaded 
        <div className="app-div">
          <h1>{user.name}</h1>

          {(user.id == sessionStorage.getItem("savedUserID"))?
            <>  
              {(isEditing)? 
                <Edit
                  user={user}
                  refresh={refresh}
                /> : null}
              <button class='rounded-button' onClick={() => changeEditState()}>
                Изменить
              </button>
            </>
            :
            null
          }
        </div>
      :  
        //if not
        <p>{"Sorry, I messed up the loading :( text me @rubidiumoxide"}</p>    
    );
}
