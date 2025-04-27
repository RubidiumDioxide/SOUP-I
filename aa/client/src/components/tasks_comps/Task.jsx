import React from 'react'; 
import {Link} from 'react-router-dom'; 

export default function Task({task, task:{id, projectId, creatorId, creatorName, assigneeId, assigneeName, name, description, isComplete}}) {
  return (
    <tr key={id}>
      <td>
        <Link to={`/task/${id}`}>{name}</Link>       
      </td>
      <td>{description}</td>
      <td>
        <Link to={`/user/${assigneeId}`}>{assigneeName}</Link>
      </td>
      <td>{(isComplete===true)?"Done":"In progress"}</td>  
    </tr>
  )
} 