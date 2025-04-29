import React, { useState } from 'react'

export default function Finish({taskId, onAction}) {

    function handleFinishClick(e) { 
      e.preventDefault();

      const result = confirm(`Это действие необратимое. Вы уверены, что хотите закончить работу над задачей?`); 

      if(result){
          fetch(`/api/Tasks/Complete/${taskId}`, {
              method : "PATCH", 
              headers : {
                  "Content-Type" : "application/json"
              },  
              body: {
                taskId : taskId 
              }
          })
              .then(onAction)
      }
    }

    return (
      <button className='rounded-button' onClick={handleFinishClick}>Завершить</button>
    )
} 