import {React, useEffect, useState} from 'react'; 
import {Link} from 'react-router-dom'; 


export default function Action({action, action:{id, projectId, projectName, actorId, actorName, taskId, taskName, description, commit, date}, type}) {
  const [repository,  setRepository] = useState(null); 

  useEffect(()=>{
    fetch(`/api/repositories/${projectId}`)
    .then(response => response.json())
    .then(data => setRepository(data));  
  }, [])


  return (
    <tr key={id}>
      {(type != 'byproject' && type != 'bytask')?
        <td><Link to={`/project/${projectId}`}>{projectName}</Link></td>
        :
        null
      }
      {(type != 'bytask')?
        <td><Link to={`/task/${taskId}`}>{taskName}</Link></td>
        : 
        null
      }
      <td>{description}</td>
      <td><Link to={`/user/${actorId}`}>{actorName}</Link></td>
      {(commit && repository)?
        <td> <Link to="#" onClick={() => window.location.href = `https://github.com/${repository.githubCreator}/${repository.githubName}/commit/${commit}`}>{commit.slice(0, 8)}</Link></td> 
      :
      <td></td>
      }
      <td>{date}</td> 
    </tr>
  )
} 