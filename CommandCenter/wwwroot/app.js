window.downloadFile = (fileName, content) => {
    const blob = new Blob([content], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement("a");
    const url = URL.createObjectURL(blob);
    link.setAttribute("href", url);
    link.setAttribute("download", fileName);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

window.toggleTheme = () => {
    document.body.classList.toggle('light-mode');
    localStorage.setItem('theme', document.body.classList.contains('light-mode') ? 'light' : 'dark');
};

window.loadTheme = () => {
    if (localStorage.getItem('theme') === 'light') {
        document.body.classList.add('light-mode');
    }
};

window.getScratchpad = () => {
    return localStorage.getItem('pm_scratchpad') || '';
};

window.saveScratchpad = (val) => {
    localStorage.setItem('pm_scratchpad', val);
};
