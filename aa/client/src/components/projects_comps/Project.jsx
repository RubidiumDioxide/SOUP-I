import React from 'react'
import {Link} from 'react-router-dom'; 

// deconstructed props
export default function Project({project, project:{id, name, description, creatorId, creatorName, isComplete, dateBegan, dateFinished, dateDeadline}}) {
  return (
    <div className="catalog-div" key={id}>    
      <h4 align="center">
        <Link to={`/project/${id}`}>{name}</Link>
      </h4>

      <div align="center">{isComplete? `Завершен ${dateFinished? dateFinished:""}`:"В процессе"}</div>

      
      <div style={{marginTop: 20, marginBottom: 20}} align="left">
        {description}
      </div>

      <div align="right">
        Создан <Link to={`/user/${creatorId}`}>{creatorName}</Link> {dateBegan}
      </div>
    
      <div align="right">
      Дедлайн: {dateDeadline? dateDeadline:""}
      </div>
    </div>
  )
} 