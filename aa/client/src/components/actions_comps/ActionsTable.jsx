import React, { use, useEffect, useState } from "react";

import Action from "./Action"; 
import Add from "./Add"; 
import Finish from "./Finish"; 


export default function ActionsTable({projectId, taskId, actorId, isTaskComplete, type, onAction, refreshCond, isProjectComplete}) {
  const [actions, setActions] = useState([]);  
  const [isAdding, setIsAdding] = useState(false); 

  var uri
  if(type=="byproject"){
    uri = `/api/Actions/ByProject/ForDisplay/${projectId}`
  }
  if(type=="bytask"){
    uri = `/api/Actions/ByTask/ForDisplay/${taskId}`
  }
  if(type=="byactor"){
    uri = `/api/Actions/ByActor/ForDisplay/${actorId}`
  }

  useEffect(()=>{
    fetch(uri)
      .then(response => response.json())
      .then(data => setActions(data)); 
  }, [isTaskComplete])

  useEffect(()=>{
    fetch(uri)
      .then(response => response.json())
      .then(data => setActions(data));  
  }, [refreshCond])

  function changeAdding(){
    if(isFinishing){
      setIsFinishing(false); 
    }
    
    setIsAdding(!isAdding); 
  }

return (
  <div class='app-div'>
    {(type == 'bytask')?
      (!isTaskComplete && !isProjectComplete)? 
        <>
          <Finish 
            taskId={taskId} 
            onAction={onAction}  
          />

          <button class='rounded-button' onClick={changeAdding}> 
            Добавить действие
          </button>
            
          {(isAdding)?
          <Add
            projectId={projectId} 
            taskId={taskId} 
            onAction={onAction}
          />
          : 
          null}
        </> 
        :
        null
      :
      null  
    }

    {(actions.length == 0)? 
    <p>Здесь пока нет действий</p>  
    : 
    <table>
      <thead>
        <tr>
          {(type != 'byproject' && type != 'bytask')?
            <th>Project</th>
            :
            null
          }
          {(type != 'bytask')?
            <th>Task</th>
            :
            null
          }
          <th>Описание</th>
          <th>Исполнитель</th>
          <th>Коммит</th>
          <th>Дата и время</th>
        </tr>
      </thead>
      <tbody>
        {actions.map(action =>
          <Action
            key={[action.id]}
            action={action}
            type={type}
          />)}
      </tbody>
    </table>
    } 
  </div>
  );
} 