import { Typography } from '@mui/material';
import React, { useState, useRef } from 'react';

export default function Search({ childToParentSearchQuery, childToParentCardsPerPage }) {
    const [text, setText] = useState('');

    return (
        <div>
            <div class="flex justify-center bg-slate-400 p-4">
                <div class="relative items-center justify-center ps-2 pr-4 pointer-events-auto focus:outline-none">
                    <button
                        type="submit"
                        autoFocus
                        class="absolute left-10 items-center flex h-5 top-3.5 focus:outline-none"
                        onClick={() => { childToParentSearchQuery(text); }}
                        onKeyDown={(e) => (
                            e.key === 13 ? childToParentSearchQuery(text) : null 
                        )}
                    >
                        <div class="bg-[url('/search-icon.png')] bg-contain opacity-50 hover:opacity-100  p-3 border-none focus:outline-none"></div>
                    </button>
                    <span class="sr-only">Search icon</span>
                </div>
                <input id="searchInput" class="block w-4/6 p-2 ps-14 bg-slate-100 focus:outline-none text-gray-900 border rounded-md text-lg"
                    value={text}
                    type="text"
                    onChange={(event) => setText(event.target.value)}
                    placeholder="Type your query i.e. 'snowboarding helmets under $100'"
                />
                <select class="pt-2 ml-2 p-2 bg-slate-400 border-slate-500 border-2 text-slate-600 text-md rounded-md  font-semibold focus:outline-none" name="cardsPerPage" onChange={(event) => childToParentCardsPerPage(event.target.value)}>
                    <option value={10} defaultValue={10} class="font-semibold">10</option>
                    <option value={20} class="font-semibold">20</option>
                    <option value={50} class="font-semibold">50</option>
                    <option value={100} class="font-semibold">100</option>
                </select>
            </div>
        </div>
    )
}