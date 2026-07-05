// Ticketora Platformu Arayüz Etkileşim Dosyası (Backend Uyumlu)
// Bu dosya DOM enjeksiyonu yapmaz; sadece sekme geçişleri ve adım kontrollerini kontrol eder.

document.addEventListener('DOMContentLoaded', () => {
  // 1. NAVİGASYON HESAP MENÜSÜ AÇILIR LİSTESİ
  initNavDropdown();

  // 2. SAYFA BAZLI ETKİLEŞİM BAŞLATICILARI
  const path = window.location.pathname;

  if (path.includes('checkout.html') || path.toLowerCase().includes('/booking/checkout')) {
    initCheckoutWizard();
  } else if (path.includes('dashboard.html') || path.toLowerCase().startsWith('/dashboard')) {
    window.initDashboardTabs?.();
  } else if (path.includes('auth.html') || path.toLowerCase().startsWith('/auth')) {
    initAuthForms();
  }
});

// ==============================================
// NAVİGASYON PROFiL MENÜSÜ GÖSTER/GİZLE
// ==============================================
function initNavDropdown() {
  const profileTrigger = document.getElementById('navProfileTrigger');
  const dropdownMenu = document.getElementById('navProfileDropdown');
  
  if (profileTrigger && dropdownMenu) {
    // Mobil veya tıklama ile açılma durumları için click desteği
    profileTrigger.addEventListener('click', (e) => {
      e.stopPropagation();
      dropdownMenu.classList.toggle('opacity-0');
      dropdownMenu.classList.toggle('pointer-events-none');
    });

    document.addEventListener('click', () => {
      dropdownMenu.classList.add('opacity-0');
      dropdownMenu.classList.add('pointer-events-none');
    });
  }
}

// ==============================================
// BİLET SATIN ALMA İŞLEMİ (DOĞRUDAN ÖDEME VE YÖNLENDİRME)
// ==============================================
function initCheckoutWizard() {
  const payBtn = document.getElementById('payBtn');
  
  if (payBtn) {
    payBtn.addEventListener('click', () => {
      processPaymentSimulation();
    });
  }

  // Kart aynalama (real-time mirror) işlemleri
  const cardNameInput = document.getElementById('cardNameInput');
  const cardNameVisual = document.getElementById('cardNameVisual');
  if (cardNameInput && cardNameVisual) {
    cardNameInput.addEventListener('input', (e) => {
      cardNameVisual.textContent = e.target.value.trim().toUpperCase() || 'AD SOYAD';
    });
  }

  const cardNumInput = document.getElementById('cardNumInput');
  const cardNumVisual = document.getElementById('cardNumVisual');
  if (cardNumInput && cardNumVisual) {
    cardNumInput.addEventListener('input', (e) => {
      let val = e.target.value.replace(/\s+/g, '').replace(/[^0-9]/gi, '');
      // Her 4 hanede bir boşluk ekle
      let formatted = '';
      for (let i = 0; i < val.length; i++) {
        if (i > 0 && i % 4 === 0) {
          formatted += ' ';
        }
        formatted += val[i];
      }
      e.target.value = formatted;
      cardNumVisual.textContent = formatted || '•••• •••• •••• 4820';
    });
  }

  const cardExpiryInput = document.getElementById('cardExpiryInput');
  const cardExpiryVisual = document.getElementById('cardExpiryVisual');
  if (cardExpiryInput && cardExpiryVisual) {
    cardExpiryInput.addEventListener('input', (e) => {
      let val = e.target.value.replace(/\s+/g, '').replace(/[^0-9]/gi, '');
      if (val.length > 2) {
        val = val.substring(0, 2) + '/' + val.substring(2, 4);
      }
      e.target.value = val;
      cardExpiryVisual.textContent = val || '12/29';
    });
  }

  const cardCvvInput = document.getElementById('cardCvvInput');
  const cardCvvVisual = document.getElementById('cardCvvVisual');
  if (cardCvvInput && cardCvvVisual) {
    cardCvvInput.addEventListener('input', (e) => {
      let val = e.target.value.replace(/[^0-9]/gi, '');
      e.target.value = val;
      cardCvvVisual.textContent = val || '•••';
    });
  }
}

function processPaymentSimulation() {
  const payBtn = document.getElementById('payBtn');
  if (payBtn) {
    payBtn.disabled = true;
    payBtn.innerHTML = `
      <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white inline-block" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
      Kart Doğrulanıyor...
    `;
  }

  setTimeout(() => {
    window.location.href = '/Booking/Success';
  }, 1500);
}

// ==============================================
// KULLANICI PANELİ DETAY MODAL İŞLEMLERİ
// ==============================================

window.initDashboardTabs = function() {
  // Dashboard sayfaları MVC action'larıyla ayrıldığı için sekme geçişi route üzerinden yapılır.
};

// Bilet Detay Modalı Tetikleyicileri
window.viewActiveTicket = function(ticketId) {
  // Bilet Detay Modalını Aç
  const modal = document.getElementById('ticketDetailModal');
  if (modal) {
    modal.classList.remove('hidden');
    modal.classList.add('flex');
  }
};

window.closeTicketModal = function() {
  const modal = document.getElementById('ticketDetailModal');
  if (modal) modal.classList.add('hidden');
};

// ==============================================
// GİRİŞ / KAYIT SAYFASI SEKME KONTROLLERİ
// ==============================================
let activeAuthTab = 'login';

function initAuthForms() {
  const wrapper = document.getElementById('mainWrapper');
  if (wrapper && wrapper.dataset.authTab) {
    activeAuthTab = wrapper.dataset.authTab;
  }

  updateAuthFormsUI();
}

window.toggleAuthView = function(view) {
  activeAuthTab = view;
  updateAuthFormsUI();
};

function updateAuthFormsUI() {
  const formTitle = document.getElementById('authFormTitle');
  const formSubtitle = document.getElementById('authFormSubtitle');
  const authToggleMsg = document.getElementById('authToggleMessage');
  const registerFields = document.getElementById('authRegisterOnlyFields');
  const loginForm = document.getElementById('authLoginForm');
  const registerForm = document.getElementById('authRegisterForm');
  const loginSubmitBtn = document.getElementById('authLoginSubmitBtn');
  const registerSubmitBtn = document.getElementById('authRegisterSubmitBtn');

  if (!formTitle) return;

  if (activeAuthTab === 'login') {
    formTitle.textContent = 'Tekrar Hoş Geldiniz';
    formSubtitle.textContent = 'Biletlerinize ve geçiş kartlarınıza erişmek için giriş yapın';
    if (loginSubmitBtn) loginSubmitBtn.textContent = 'Giriş Yap';
    if (loginForm) loginForm.classList.remove('hidden');
    if (registerForm) registerForm.classList.add('hidden');
    if (registerFields) registerFields.classList.add('hidden');
    if (authToggleMsg) {
      authToggleMsg.innerHTML = `Hesabınız yok mu? <button type="button" onclick="toggleAuthView('register')" class="text-indigo-400 font-bold hover:underline cursor-pointer">Kayıt Olun</button>`;
    }
  } else {
    formTitle.textContent = 'Hesap Oluşturun';
    formSubtitle.textContent = 'Saniyeler içinde resmi bilet cüzdanınızı oluşturun';
    if (registerSubmitBtn) registerSubmitBtn.textContent = 'Cüzdanı Oluştur';
    if (loginForm) loginForm.classList.add('hidden');
    if (registerForm) registerForm.classList.remove('hidden');
    if (registerFields) registerFields.classList.remove('hidden');
    if (authToggleMsg) {
      authToggleMsg.innerHTML = `Zaten hesabınız var mı? <button type="button" onclick="toggleAuthView('login')" class="text-indigo-400 font-bold hover:underline cursor-pointer">Giriş Yapın</button>`;
    }
  }
}
