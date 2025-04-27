import React, { useEffect, useState} from "react";
import { Link } from 'react-router-dom'; 
import Project from "./Project"; 
import Search from './Search'; 


export default function ProjetcsTable({type}) {
  const [projects, setProjects] = useState(null);
  const [refreshCond, setRefreshCond] = useState([false]);
  const userId = sessionStorage.getItem("savedUserID");
  
  var uri = (type=="my")?`/api/Projects/ForDisplay/Participants/${userId}`:'/api/Projects/ForDisplay/Public'; 

  useEffect(()=>{
    fetch(uri)
    .then(response => {
      if (response.status === 404) {
        console.error('Projects not found.');
      }
      return response.json();
    })
    .then(data => {
      if (data != undefined && data != null) {
        setProjects(data); 
      }
    })
    .catch(error => {
      console.error('Fetch error:', error);
    });
    setRefreshCond([false]); 
  }, refreshCond)
  


  function onAction(){
    setRefreshCond([true]); 
  }

  function onSearch(searchForm){
     console.log(searchForm);

        fetch('/api/Projects/Search/ForDisplay/Public', { 
          method: "POST",
          headers: {
              "Content-Type" : "application/json"
          },
          body: JSON.stringify(searchForm) 
          })
          .then(response => {
            if (response.status === 404) {
              console.error('Projects not found.');
            }
            return response.json();
          })
          .then(data => {
            if (data != undefined && data != null) {
              setProjects(data); 
            }
          })
          .catch(error => {
            console.error('Fetch error:', error);
          });
    } 

  return (
    projects?
    <div className="app-div">
    {console.log(projects)}
      {(type == "all")?
        <Search
          onSearch={onSearch}
        />
        :
        null
      }

      {(type=="my")? 
      <button class='rounded-button'>
        <Link to="/newproject">Новый проект</Link>
      </button>
      :
      null
      }

      {(projects.length == 0)? 
      <p>No projects yet</p>  
      :
      <div className="catalog">
        {projects.map(project =>
          <Project
            key={project.id}
            project={project}
          />)}
      </div>
    } 
  </div>
  :
  <p>{"Sorry, I messed up the loading :( text me @rubidiumoxide"}</p>  
  )
}
