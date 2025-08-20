import { WhatsAppProvider } from "./providers/whatsapp.provider";

const sessioName = 'bot_whats_ativo_001';

const whatsAppProvider = new WhatsAppProvider(sessioName);

whatsAppProvider.init();

