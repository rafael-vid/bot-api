import { WhatsAppProvider } from "./providers/whatsapp.provider";

const sessioName = 'bot_whats_passivo_001';

const whatsAppProvider = new WhatsAppProvider(sessioName);

whatsAppProvider.init();

