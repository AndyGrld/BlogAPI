// // fetch one user
// fetch("https://localhost:7017/api/User/1")
// .then( res => res.json())
// .then( data => console.log(data));

// // create a user
// fetch("https://localhost:7017/api/User", {
//     method: 'POST',
//     headers: {
//       'Content-Type': 'application/json',
//     },
//     body: JSON.stringify({
//         username: 'john doe',
//         email: 'johnny@gmail.com',
//         password: 'password'
//       }),
//   })
//     .then(response => {
//         return response.json();
//     })
//     .then(data => {
//       console.log('Success:', data);
//       // Handle the response data here
//     })
//     .catch(error => {
//       console.error('Error:', error);
//       // Handle errors here
// });

// // fetch all users
// fetch("https://localhost:7017/api/User")
// .then( res => res.json())
// .then( data => console.log(data));

// // delete a user
// fetch("https://localhost:7017/api/User/2",
//     {method: 'DELETE'}
//     )
//     .then(res => res.json())
//     .then(data => console.log(data))

// // update a user
// fetch("https://localhost:7017/api/User/5", {
//     method: "PUT",
//     headers: {
//         'Content-Type': 'application/json',
//     },
//     body: JSON.stringify({
//         userId: 5,
//         username: "Gerald1",
//         email: 'ansongagerld@yahoo.com'
//     })
// })
// .then(res => res.json())
// .then(data => console.log(data))