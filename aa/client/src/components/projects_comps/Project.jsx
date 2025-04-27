import React from 'react'
import {Link} from 'react-router-dom'; 

// deconstructed props
export default function Project({project, project:{id, name, description, creatorId, creatorName, isComplete, dateBegan, dateFinished, dateDeadline}}) {
  return (



    <div className="catalog-div" key={id}>    
      <h4 align="center"><Link to={`/project/${id}`}>{name}</Link>
      </h4>

      <div align="left">
        {description}
      </div>

      <div align="right">
        Создан <Link to={`/user/${creatorId}`}>{creatorName}</Link>
        <div>{dateBegan}</div>
      </div>
      
      <div align="center">{isComplete? "Finished":"In progress"}

      </div>
  
      <div>
      {dateFinished? dateFinished:""}
      </div>
      <div>
      Дедлайн: {dateDeadline? dateDeadline:""}
      </div>
    </div>
  )
} 