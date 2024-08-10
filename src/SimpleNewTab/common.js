"use strict";

const migrations = [
    (storage) => {
        storage.shortcuts = [
            { 
                name: "GitHub", 
                url: "https://github.com/" 
            },
            { 
                name: "Stack Overflow", 
                url: "https://stackoverflow.com/" 
            },
            { 
                name: "MDN", 
                url: "https://developer.mozilla.org/en-US/" 
            }
        ];
    }
];

async function migrate() {
    let storage = await chrome.storage.local.get();
    if (storage.migrations === migrations.length) {
        return storage;
    }
    for (let i = storage.migrations ?? 0; i < migrations.length; i++) {
        migrations[i](storage);
    }
    storage.migrations = migrations.length;
    await chrome.storage.local.clear();
    await chrome.storage.local.set(storage);
    return storage;
}

function q(selector, context) {
    if (selector.startsWith("#")) {
        return document.querySelector(selector);
    } else if (context) {
        return context.querySelector(selector);
    } else {
        return document.querySelectorAll(selector);
    }
}
