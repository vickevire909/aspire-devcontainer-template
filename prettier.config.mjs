/** @type {import('prettier').Config} */
export default {
    plugins: ["prettier-plugin-tailwindcss"],
    overrides: [
        {
            files: "src/Apps/storefront/**/*.{js,ts,tsx,css}",
            options: {
                tailwindStylesheet: "./src/Apps/storefront/src/styles.css",
            },
        },
    ],
};
