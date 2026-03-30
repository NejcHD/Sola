// JS/speech.js - Basic Voice Interface (ENGLISH)

class SpeechManager {
    constructor() {
        this.recognition = null;
        this.synthesis = window.speechSynthesis;
        this.isListening = false;
        this.currentCommands = [];
        this.state = 'main';
        this.selectedPostIndex = -1;

        this.initSpeechRecognition();
        this.setupCommands();
    }

    initSpeechRecognition() {
        if ('webkitSpeechRecognition' in window) {
            this.recognition = new webkitSpeechRecognition();
            this.recognition.continuous = true;
            this.recognition.interimResults = false;
            this.recognition.lang = 'en-US';

            this.recognition.onresult = (event) => {
                const transcript = event.results[event.resultIndex][0].transcript.toLowerCase();
                console.log("Recognized:", transcript);
                this.processCommand(transcript);
            };

            this.recognition.onerror = (event) => {
                console.error("Recognition error:", event.error);
            };

            this.recognition.onend = () => {
                this.isListening = false;
                // Auto-restart
                setTimeout(() => {
                    if (this.recognition) {
                        this.recognition.start();
                    }
                }, 100);
            };

        } else {
            console.error("Web Speech API not supported.");
        }
    }

    setupCommands() {
        // Main screen commands
        this.mainCommands = [
            { pattern: /open settings|settings/i, action: 'openSettings', response: 'Opening settings' },
            { pattern: /dark theme|dark mode/i, action: 'darkTheme', response: 'Switched to dark theme' },
            { pattern: /light theme|light mode/i, action: 'lightTheme', response: 'Switched to light theme' },
            { pattern: /load data|load posts/i, action: 'loadData', response: 'Opening file dialog' },
            { pattern: /select post|choose post/i, action: 'selectPost', response: 'Please say post number' },
            { pattern: /help|commands/i, action: 'showHelp', response: 'Showing available commands' },
            { pattern: /test voice|testing/i, action: 'testVoice', response: 'Voice interface is working correctly' }
        ];

        // When post is selected
        this.postCommands = [
            { pattern: /open post|show post/i, action: 'openPost', response: 'Opening selected post' },
            { pattern: /add like|like post/i, action: 'likePost', response: 'Adding like to post' },
            { pattern: /back|main screen/i, action: 'backToMain', response: 'Returning to main screen' },
            { pattern: /first post/i, action: 'selectFirst', response: 'Selected first post' },
            { pattern: /last post/i, action: 'selectLast', response: 'Selected last post' },
            { pattern: /delete post|remove post/i, action: 'deletePost', response: 'Deleting post' },
            { pattern: /edit post|change post/i, action: 'editPost', response: 'Editing post' }
        ];

        this.currentCommands = this.mainCommands;
    }

    processCommand(transcript) {
        let commandFound = false;

        for (const command of this.currentCommands) {
            if (command.pattern.test(transcript)) {
                commandFound = true;
                this.executeCommand(command.action, transcript);
                this.speak(command.response);
                break;
            }
        }

        if (!commandFound) {
            this.speak("Unknown command. Try again.");
        }
    }

    executeCommand(action, transcript) {
        console.log("Executing command:", action);

        switch(action) {
            case 'openSettings':
                if (window.electronAPI && window.electronAPI.openSettingsWindow) {
                    window.electronAPI.openSettingsWindow();
                }
                break;

            case 'darkTheme':
                this.changeTheme('dark');
                break;

            case 'lightTheme':
                this.changeTheme('light');
                break;

            case 'loadData':
                if (window.openFileDialog) {
                    window.openFileDialog();
                }
                break;

            case 'selectPost':
                this.speak("Please say post number from 1 to " + (window.vseObjave?.length || 0));
                this.state = 'selectingPost';
                break;

            case 'showHelp':
                this.showAvailableCommands();
                break;

            case 'openPost':
                if (this.selectedPostIndex !== -1) {
                    this.openPostDetails(this.selectedPostIndex);
                }
                break;

            case 'likePost':
                if (this.selectedPostIndex !== -1) {
                    this.likeCurrentPost();
                }
                break;

            case 'deletePost':
                if (this.selectedPostIndex !== -1) {
                    this.deleteCurrentPost();
                }
                break;

            case 'editPost':
                if (this.selectedPostIndex !== -1) {
                    this.editCurrentPost();
                }
                break;

            case 'backToMain':
                this.state = 'main';
                this.selectedPostIndex = -1;
                this.currentCommands = this.mainCommands;
                this.updateCommandDisplay();
                break;

            case 'selectFirst':
                this.selectedPostIndex = 0;
                this.state = 'postSelected';
                this.currentCommands = this.postCommands;
                this.updateCommandDisplay();
                this.speak("Selected first post: " + (window.vseObjave[0]?.naslov || ""));
                break;

            case 'selectLast':
                if (window.vseObjave && window.vseObjave.length > 0) {
                    this.selectedPostIndex = window.vseObjave.length - 1;
                    this.state = 'postSelected';
                    this.currentCommands = this.postCommands;
                    this.updateCommandDisplay();
                    this.speak("Selected last post: " + window.vseObjave[this.selectedPostIndex]?.naslov);
                }
                break;

            case 'testVoice':
                this.speak("This is a test message. If you can hear this, voice output is working.");
                break;
        }

        // Check numbers for post selection
        if (this.state === 'selectingPost') {
            const numMatch = transcript.match(/\d+/);
            if (numMatch) {
                const postNum = parseInt(numMatch[0]) - 1;
                if (window.vseObjave && postNum >= 0 && postNum < window.vseObjave.length) {
                    this.selectedPostIndex = postNum;
                    this.state = 'postSelected';
                    this.currentCommands = this.postCommands;
                    this.updateCommandDisplay();
                    this.speak("Selected post " + (postNum + 1) + ": " + window.vseObjave[postNum].naslov);
                }
            }
        }
    }

    changeTheme(theme) {
        const themeLink = document.getElementById('theme-css');
        const themeBtn = document.getElementById('toggle-theme');

        if (themeLink) {
            themeLink.href = `StyleCss/${theme}.css`;
        }

        if (themeBtn) {
            themeBtn.textContent = theme === 'light' ? '🌙 Dark theme' : '☀️ Light theme';
        }

        localStorage.setItem('theme', theme);

        if (window.electronAPI && window.electronAPI.changeTheme) {
            window.electronAPI.changeTheme(theme);
        }
    }

    openPostDetails(index) {
        const post = window.vseObjave[index];
        if (post) {
            alert(`Post details:\nTitle: ${post.naslov}\nAuthor: ${post.avtor}\nContent: ${post.vsebina}`);
        }
    }

    likeCurrentPost() {
        if (window.vseObjave && this.selectedPostIndex !== -1) {
            window.vseObjave[this.selectedPostIndex].vsec =
                (window.vseObjave[this.selectedPostIndex].vsec || 0) + 1;

            if (window.osveziPrikaz) {
                window.osveziPrikaz(window.vseObjave);
            }

            this.speak("Like added");
        }
    }

    deleteCurrentPost() {
        if (window.vseObjave && this.selectedPostIndex !== -1) {
            const postTitle = window.vseObjave[this.selectedPostIndex].naslov;
            window.vseObjave.splice(this.selectedPostIndex, 1);
            this.speak("Post '" + postTitle + "' deleted");

            this.state = 'main';
            this.selectedPostIndex = -1;
            this.currentCommands = this.mainCommands;
            this.updateCommandDisplay();

            if (window.osveziPrikaz) {
                window.osveziPrikaz(window.vseObjave);
            }
        }
    }

    editCurrentPost() {
        if (window.vseObjave && this.selectedPostIndex !== -1) {
            const post = window.vseObjave[this.selectedPostIndex];
            const newTitle = prompt("New title:", post.naslov);
            if (newTitle) {
                post.naslov = newTitle;
                this.speak("Title changed to: " + newTitle);
                if (window.osveziPrikaz) window.osveziPrikaz(window.vseObjave);
            }
        }
    }

    speak(text) {
        if (this.synthesis) {
            const utterance = new SpeechSynthesisUtterance(text);
            utterance.lang = 'en-US';
            utterance.rate = 1.0;
            utterance.pitch = 1.0;
            this.synthesis.speak(utterance);
        }
    }

    start() {
        if (this.recognition && !this.isListening) {
            this.recognition.start();
            this.isListening = true;
            console.log("Starting to listen...");
            this.updateCommandDisplay();
        }
    }

    stop() {
        if (this.recognition && this.isListening) {
            this.recognition.stop();
            this.isListening = false;
            console.log("Stopping listening...");
            this.updateCommandDisplay();
        }
    }

    showAvailableCommands() {
        let message = "Available commands: ";
        this.currentCommands.forEach((cmd, i) => {
            const patternText = cmd.pattern.toString().match(/\/(.*?)\//)[1];
            message += patternText + (i < this.currentCommands.length - 1 ? ", " : "");
        });
        this.speak(message);
    }

    updateCommandDisplay() {
        const display = document.getElementById('voice-commands-display');
        if (!display) return;

        let html = '<h3>🎤 Voice commands:</h3>';
        html += '<p>Status: ' + (this.isListening ? '🎧 Listening...' : '🔇 Not listening') + '</p>';
        html += '<p>State: ' + (this.state === 'main' ? 'Main screen' :
            this.state === 'selectingPost' ? 'Selecting post' : 'Post selected') + '</p>';

        if (this.selectedPostIndex !== -1 && window.vseObjave && window.vseObjave[this.selectedPostIndex]) {
            html += '<p>Selected post: ' + window.vseObjave[this.selectedPostIndex].naslov + '</p>';
        }

        html += '<div class="commands-list">';
        this.currentCommands.forEach(cmd => {
            const patternText = cmd.pattern.toString().match(/\/(.*?)\//)[1];
            html += `<div class="voice-command">"${patternText}"</div>`;
        });
        html += '</div>';

        display.innerHTML = html;
    }

    toggleCommandDisplay() {
        const display = document.getElementById('voice-commands-display');
        const toggleBtn = document.getElementById('toggle-display');

        if (display && toggleBtn) {
            if (display.style.display === 'none' || display.style.display === '') {
                display.style.display = 'block';
                toggleBtn.textContent = '👁️ Hide display';
                this.speak("Command display opened");
            } else {
                display.style.display = 'none';
                toggleBtn.textContent = '👁️ Show display';
                this.speak("Command display closed");
            }
        }
    }
}

let speechManager = null;

document.addEventListener('DOMContentLoaded', function() {
    const commandDisplay = document.createElement('div');
    commandDisplay.id = 'voice-commands-display';
    commandDisplay.style.cssText = `
        position: fixed;
        bottom: 20px;
        right: 20px;
        width: 320px;
        background: var(--card-color);
        border: 1px solid var(--border-color);
        border-radius: 10px;
        padding: 15px;
        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        z-index: 1000;
        max-height: 400px;
        overflow-y: auto;
    `;
    document.body.appendChild(commandDisplay);

    const voiceControlDiv = document.createElement('div');
    voiceControlDiv.className = 'voice-controls';
    voiceControlDiv.innerHTML = `
        <button id="start-voice" class="voice-btn">🎤 Start listening</button>
        <button id="stop-voice" class="voice-btn">🔇 Stop listening</button>
        <button id="toggle-display" class="voice-btn">👁️ Hide display</button>
        <button id="clear-selection" class="voice-btn">🗑️ Reset selection</button>
    `;
    voiceControlDiv.style.cssText = `
        position: fixed;
        bottom: 20px;
        left: 20px;
        display: flex;
        gap: 10px;
        flex-wrap: wrap;
        z-index: 1000;
    `;
    document.body.appendChild(voiceControlDiv);

    speechManager = new SpeechManager();

    document.getElementById('start-voice').addEventListener('click', () => {
        speechManager.start();
        speechManager.speak("Voice interface activated");
    });

    document.getElementById('stop-voice').addEventListener('click', () => {
        speechManager.stop();
        speechManager.speak("Voice interface deactivated");
    });

    document.getElementById('toggle-display').addEventListener('click', () => {
        speechManager.toggleCommandDisplay();
    });

    document.getElementById('clear-selection').addEventListener('click', () => {
        document.querySelectorAll('.objava').forEach(o => o.classList.remove('selected'));

        if (speechManager) {
            speechManager.state = 'main';
            speechManager.selectedPostIndex = -1;
            speechManager.currentCommands = speechManager.mainCommands;
            speechManager.updateCommandDisplay();
            speechManager.speak("Selection reset");
        }
    });

    const style = document.createElement('style');
    style.textContent = `
        .voice-btn {
            padding: 10px 15px;
            background-color: var(--Tema-button);
            color: var(--text-color);
            border: 1px solid var(--border-color);
            border-radius: 5px;
            cursor: pointer;
            font-size: 14px;
        }
        
        .voice-btn:hover {
            opacity: 0.8;
        }
        
        .commands-list {
            margin-top: 10px;
        }
        
        .voice-command {
            background-color: rgba(52, 152, 219, 0.1);
            border: 1px solid rgba(52, 152, 219, 0.3);
            border-radius: 5px;
            padding: 8px 12px;
            margin: 5px 0;
            font-family: monospace;
            font-size: 14px;
        }
        
        #voice-commands-display h3 {
            margin-top: 0;
            color: var(--primary-color);
            border-bottom: 2px solid var(--primary-color);
            padding-bottom: 5px;
        }
    `;
    document.head.appendChild(style);

    speechManager.updateCommandDisplay();

    setTimeout(() => {
        speechManager.start();
        speechManager.speak("Social network ready. Say help for command list.");
    }, 1000);
});

window.speechManager = speechManager;