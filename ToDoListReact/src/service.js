import axios from 'axios';

const apiUrl = process.env.REACT_APP_API_URL;
// const apiUrl ="https://localhost:7080"

export default {
  getTasks: async () => {
    console.log(process.env.REACT_APP_API_URL);
    const result = await axios.get(`${apiUrl}/tasks`)    
    return result.data;
  },


  addTask: async(name)=>{
    console.log('addTask', name)
    const result = await axios.post(`${apiUrl}/tasks`,{name})    
    return result.data;
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    const result = await axios.put(`${apiUrl}/tasks/${id}`,{isComplete})    
    return result.data;
  },

  deleteTask:async(id)=>{
    console.log('deleteTask')
    const result = await axios.delete(`${apiUrl}/tasks/${id}`)    
    return result.data;
  }
};
