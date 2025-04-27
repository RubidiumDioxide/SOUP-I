import React, { useEffect, useState } from "react";
import Team from "./Team"; 
import Add from "./Invite"; 
import ChangeRole from "./ChangeRole";

export default function TeamsTable({isCreator, projectId, isProjectComplete}) {
  const [teams, setTeams] = useState([]);
  const [refreshCond, setRefreshCond] = useState([false]);  
  const [isEditing, setIsEditing] = useState(false); 
  const [isAdding, setIsAdding] = useState(false); 
  const [capturedTeam, setCapturedTeam] = useState(null);

  var uri = `/api/Teams/ForDisplay/Project/${projectId}`; 

  useEffect(()=>{
    fetch(uri)
    .then(response => response.json())
    .then(data => setTeams(data)); 
    setRefreshCond([false]); 
  }, refreshCond)

  function onAction(){
    if(isAdding){
      changeAddState(); 
    }
    if(isEditing){
      changeEditState(); 
    }
    setRefreshCond([true]); 
  }

  function changeAddState(){
    setIsAdding(!isAdding); 
  }

  function changeEditState(){
    setIsEditing(!isEditing); 
  }

  function captureEdit(clickedTeam){ 
    changeEditState(); 
    setCapturedTeam(clickedTeam); 
  }

return (
  <div className="app-div">
    {(isCreator && !isProjectComplete)? 
    <div>
      <button class='rounded-button' onClick={changeAddState}>
        Invite
      </button>
      
      {isAdding?
      (<Add
          projectId={projectId}
          onAction={onAction}
        />)
      : null}

      {isEditing? 
      (<ChangeRole
          team={capturedTeam}
          onAction={onAction}
        />)
      : null}
    </div> 
    : 
    null}


    {(teams.length == 0)? 
    <p>No members yet</p>  
    :
    <table>
      <thead>
        <tr>
          <th>Имя</th>
          <th>Роль</th>
          <th></th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        {teams.map(team =>
          <Team
            key={[team.id]}
            team={team}
            onAction={onAction}
            captureEdit={captureEdit}
            isCreator={isCreator}
            isProjectComplete={isProjectComplete}
          />)}
      </tbody>
    </table>
    }
   
  </div>
  );
} 