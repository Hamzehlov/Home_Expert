// Language Toggle Function
function toggleLanguage() {
    const htmlRoot = document.getElementById('htmlRoot');
    const currentLang = htmlRoot.getAttribute('lang');
    const currentDir = htmlRoot.getAttribute('dir');
    
    if (currentLang === 'en') {
        htmlRoot.setAttribute('lang', 'ar');
        htmlRoot.setAttribute('dir', 'rtl');
        // Save preference to localStorage
        localStorage.setItem('language', 'ar');
    } else {
        htmlRoot.setAttribute('lang', 'en');
        htmlRoot.setAttribute('dir', 'ltr');
        // Save preference to localStorage
        localStorage.setItem('language', 'en');
    }
}

// Password Toggle Function
function togglePassword() {
    const passwordInput = document.getElementById('passwordInput');
    const passwordIcon = document.getElementById('passwordIcon');
    
    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        passwordIcon.textContent = 'visibility_off';
    } else {
        passwordInput.type = 'password';
        passwordIcon.textContent = 'visibility';
    }
}

// Load saved language preference on page load
document.addEventListener('DOMContentLoaded', function() {
    const savedLanguage = localStorage.getItem('language');
    const htmlRoot = document.getElementById('htmlRoot');
    
    if (savedLanguage === 'ar') {
        htmlRoot.setAttribute('lang', 'ar');
        htmlRoot.setAttribute('dir', 'rtl');
    } else {
        htmlRoot.setAttribute('lang', 'en');
        htmlRoot.setAttribute('dir', 'ltr');
    }
    
    // Form submission handler (you can customize this)
    const form = document.getElementById('registrationForm');
    form.addEventListener('submit', function(e) {
        e.preventDefault();
        
        // Get form values
        const formData = new FormData(form);
        
        // Here you would typically send the data to your server
        console.log('Form submitted with data:');
        for (let [key, value] of formData.entries()) {
            console.log(key + ': ' + value);
        }
        
        // Show success message or redirect
        alert('Form submitted successfully!');
    });
});

// Add smooth transitions when changing language
const htmlRoot = document.getElementById('htmlRoot');
const observer = new MutationObserver(function(mutations) {
    mutations.forEach(function(mutation) {
        if (mutation.attributeName === 'dir') {
            document.body.style.transition = 'all 0.3s ease';
        }
    });
});

observer.observe(htmlRoot, {
    attributes: true
});
