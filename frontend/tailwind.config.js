/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{vue, js,ts,jsx,tsx}'],
  darkMode: false,
  theme: {
    extend: {
      colors: {
        primary: {
          100: '#E9FAFF',
          200: '#CBE6EE',
          300: '#A7D0DC',
          400: '#8BBDCC',
          500: '#1C596C',
          600: '#77AFC0',
          700: '#448193',
          800: '#1B5768',
          900: '#042E39',
          '90%': '#1C596CE5',
          '80%': '#1C596CCC',
          '20%': '#1C596C33',
          '10%': '#1C596C1A',
          '5%': '#1C596C0D',
        },
        snow: {
          '90%': '#FFFFFFE5',
          '80%': '#FFFFFFCC',
          '20%': '#FFFFFF33',
          '10%': '#FFFFFF1A',
          '5%': '#FFFFFF0D',
        },
        dark: {
          500: '#100F14',
          '90%': '#100F14E5',
          '80%': '#100F14CC',
          '20%': '#100F1433',
          '10%': '#100F141A',
          '5%': '#100F140D',
        },
        'light-gray': {
          500: '#D9E1E1',
          '90%': '#D9E1E1E5',
          '80%': '#D9E1E1CC',
          '20%': '#D9E1E133',
          '10%': '#D9E1E11A',
          '5%': '#D9E1E10D',
        },
        gray: {
          '90%': '#A8A7AEE5',
          '80%': '#A8A7AECC',
          '20%': '#A8A7AE33',
          '10%': '#A8A7AE1A',
          '5%': '#A8A7AE0D',
        },
        brand: '#DAE6E9',
        'brand-gray': '#61727D',
      },
      fontFamily: {
        montserrat: 'Montserrat',
        manrope: 'Manrope',
      },
      borderWidth: {
        3: '3px',
      },
      ringWidth: {
        3: '3px',
      },
      strokeWidth: {
        1.5: '1.5px',
        3: '3px',
        4: '4px',
        5: '5px',
        6: '6px',
      },
      screens: {
        '2xs': '405px',
        xs: '475px',
      },
    },
  },
  // eslint-disable-next-line no-undef
  plugins: [require('@tailwindcss/forms')],
};
