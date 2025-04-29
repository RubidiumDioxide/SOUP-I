import React, { useState } from 'react'

export default function Finish({project, onAction}) {

    function handleFinishClick(e) { 
      e.preventDefault();

      const result = confirm(`Это действие необратимое. Вы уверены, что хотите закончить работу над проектом ${project.name}?`); 

      if(result){
        fetch(`/api/Projects/Finish/${project.id}`, {
          method : "PATCH", 
          headers : {
              "Content-Type" : "application/json"
          },             
          body : JSON.stringify({
            isComplete: true 
          })
          })
          .then(response => {
            if (!response.ok) {
              throw new Error(`Error finishing project: ${response.status} ${response.statusText}`);
            }
            console.log('Project finished successfully.');
            onAction(); 
          })
          .catch(error => {
            console.error('Error finishing project:', error);
            alert(error.message || "Не удалось завершить проект: на серевре возникла ошибка.");
          });
      }
    }

    return (
      <button className='rounded-button' onClick={handleFinishClick}>Завершить</button>
    )
} 