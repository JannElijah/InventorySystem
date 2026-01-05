import customtkinter as ctk

DEFAULT_CONFIG = {
    "theme": "System",
    "ui_scale": 1.0,
    "company_name": "My Shop",
    "company_address": "123 Main St",
    "currency_symbol": "$",
    "tax_rate": 0.0,
    "contact_phone": "555-0199"
}

class ConfigManager:
    _db = None
    _cache = DEFAULT_CONFIG.copy()

    @classmethod
    def initialize(cls, db):
        cls._db = db
        # Load all settings to cache
        for key in DEFAULT_CONFIG.keys():
            val = cls._db.get_setting(key, str(DEFAULT_CONFIG[key]))
            # Type conversion
            if key == "ui_scale" or key == "tax_rate":
                try:
                    cls._cache[key] = float(val)
                except:
                    cls._cache[key] = DEFAULT_CONFIG[key]
            else:
                cls._cache[key] = val
        
        cls.apply_theme()
        cls.apply_scale()

    @classmethod
    def get(cls, key):
        return cls._cache.get(key, DEFAULT_CONFIG.get(key))

    @classmethod
    def set(cls, key, value):
        cls._cache[key] = value
        if cls._db:
            cls._db.set_setting(key, str(value))
        
        if key == "theme": cls.apply_theme()
        if key == "ui_scale": cls.apply_scale()

    @classmethod
    def apply_theme(cls):
        ctk.set_appearance_mode(cls._cache.get("theme", "System"))

    @classmethod
    def apply_scale(cls):
        ctk.set_widget_scaling(cls._cache.get("ui_scale", 1.0))
