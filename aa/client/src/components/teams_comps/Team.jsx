import React from 'react'; 
import Delete from './Delete'; 
import {Link} from 'react-router-dom'; 

// deconstructed props
export default function Team({team, team:{id, userId, userName, projectId, projectName, role, level}, onAction, captureEdit, isCreator, isProjectComplete}) {
  return (
    <tr key={id}>
      <td>
        <Link to={`/user/${userId}`}>{userName}</Link>
      </td>
      <td>{role}</td>
      {(isCreator && !isProjectComplete)?
      <>
        <td>
         <Delete
           team={team} 
           onAction={onAction}
         />
        </td>
        <td>
          <button class='rounded-button' onClick={(e) => captureEdit(team)}>
            Изменить роль
          </button>
        </td>
      </>
      :
      null
    }   
    </tr>
  )
} 