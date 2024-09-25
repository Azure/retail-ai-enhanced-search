import React from "react";

export default function NavBar({children}) {
    return (
        <nav class="border-slate-200 dark:bg-slate-900">
            <div class="max-w-screen-xl flex flex-wrap items-center h-24 justify-between mx-auto p-0">
                <a href="https://azure.microsoft.com/en-us/products/ai-services/ai-search" class="flex items-center space-x-3 rtl:space-x-reverse">
                    <img src="logo.png" class="h-10" alt="AI Search Logo" />
                    <span class="text-3xl font-semibold whitespace-nowrap text-white">Contoso Products</span>
                </a>
                <div class="flex md:order-2">
                    
                </div>
                <div class="items-center justify-between hidden w-full md:flex md:w-auto md:order-1" id="navbar-links">
                    <ul class="flex flex-col p-4 md:p-0 mt-4 font-medium border border-slate-100 rounded-lg bg-slate-50 md:space-x-8 rtl:space-x-reverse md:flex-row md:mt-0 md:border-0 md:bg-white dark:bg-slate-800 md:dark:bg-slate-900 dark:border-slate-700">
                        <li>
                            <a href="#" class="block py-2 px-3 text-white bg-blue-700 rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500" aria-current="page">Home</a>
                        </li>
                        <li>
                            <a href="#" class="block py-2 px-3 text-slate-900 rounded hover:bg-slate-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 md:dark:hover:text-blue-500 dark:text-white dark:hover:bg-slate-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-slate-700">About</a>
                        </li>
                        <li>
                            <a href="#" class="block py-2 px-3 text-slate-900 rounded hover:bg-slate-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-slate-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-slate-700">Services</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    )
}