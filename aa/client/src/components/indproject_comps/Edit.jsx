import React, { useState } from 'react';
import convertDateFormat from '../../Extensions/convertDateFormat';

const uri = '/api/projects';

export default function Edit({ project, onAction }) {
  const [editForm, setEditForm] = useState({
    name: project.name,
    description: project.description,
    dateDeadline: convertDateFormat(project.dateDeadline),
    isPrivate: Boolean(project.isPrivate), 
  });

  function handleEditChange(e) {
    const { name, type, value, checked } = e.target; 

    setEditForm(prevForm => {
      // Handle checkbox separately
      const newValue = type === 'checkbox' ? checked : value;

      return {
        ...prevForm,
        [name]: newValue,
      };
    });
  }

  function handleEditForm(e) {
    e.preventDefault();

    console.log({
      id: project.id,
      name: editForm.name,
      description: editForm.description,
      creator: project.creator,
      isComplete: project.isComplete,
      dateBegan: project.dateBegan,
      dateFinished: project.dateFinished,
      dateDeadline: editForm.dateDeadline,
      isPrivate: editForm.isPrivate,
    });

    fetch(uri + `/${project.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        id: project.id,
        name: editForm.name,
        description: editForm.description,
        creator: project.creator,
        isComplete: Boolean(project.isComplete),
        dateBegan: project.dateBegan,
        dateFinished: project.dateFinished,
        dateDeadline: editForm.dateDeadline,
        isPrivate: editForm.isPrivate, 
      }),
    }).then(onAction);
  }

  return (
    <div>
      <h4>Edit project name & description</h4>

      <form onSubmit={handleEditForm}>
        <input
          className="rounded-input"
          type="text"
          name="name"
          value={editForm.name}
          onChange={handleEditChange}
        />
        <input
          className="rounded-input"
          type="text"
          name="description"
          value={editForm.description}
          onChange={handleEditChange}
        />
        <input
          className="rounded-input"
          type="date"
          name="dateDeadline"
          value={editForm.dateDeadline}
          onChange={handleEditChange}
        />
        <label>
          Private
          <input
            className="rounded-input"
            type="checkbox"
            name="isPrivate"
            checked={editForm.isPrivate} 
            onChange={handleEditChange}
          />
        </label>

        <button className="rounded-button" type="submit">
          Submit Changes
        </button>
      </form>
    </div>
  );
}