/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml",  // Ajustez le chemin si nécessaire
    "./Pages/**/*.cshtml",
    "./wwwroot/js/**/*.js",
  ],
  theme: {
    extend: {
      colors: {
        transparent: 'transparent',
        black: '#000',
        white: '#fff',
        beige:'#A6989A',
        grey :'#1a202c',
        bleugris:'#577C92',
        bleugrisclair:'#8FBBD3',
        vertgris:'#415C6B',
        pink:'#E1425C',
        bluenight : '#040B40', 
        red:{
          500:'#4A0013'
        }
      },
    },
  },
  plugins: [],
}
