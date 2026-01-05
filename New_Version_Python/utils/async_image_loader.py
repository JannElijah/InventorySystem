
import threading
import queue
from PIL import Image, ImageTk
import customtkinter as ctk
import os

class AsyncImageLoader:
    def __init__(self):
        self.request_queue = queue.Queue()
        self.result_queue = queue.Queue()
        self.cache = {}
        self.running = True
        self.worker_thread = threading.Thread(target=self._worker, daemon=True)
        self.worker_thread.start()

    def load_image(self, path, size, callback):
        """
        Request an image to be loaded.
        callback: function(image_tk) called on main thread
        """
        if path in self.cache:
            # If in cache, check if size matches? Simple cache for now.
            # Ideally cache by (path, size)
            key = (path, size)
            if key in self.cache:
                callback(self.cache[key])
                return

        self.request_queue.put((path, size, callback))
        self.process_results() # Check for results

    def _worker(self):
        while self.running:
            try:
                path, size, callback = self.request_queue.get(timeout=0.1)
                pil_img = self._load_image_sync(path, size)
                # Pass PIL image to result queue, NOT ImageTk
                self.result_queue.put((callback, pil_img, path, size))
                self.request_queue.task_done()
            except queue.Empty:
                continue
            except Exception as e:
                print(f"Image Load Error: {e}")

    def _load_image_sync(self, path, size):
        try:
            if not os.path.exists(path):
                return None
            
            pil_img = Image.open(path)
            pil_img.thumbnail(size)
            return pil_img # Return PIL object
        except Exception:
            return None

    def process_results(self):
        """Call this periodically or after requests"""
        try:
            while True:
                callback, pil_img, path, size_req = self.result_queue.get_nowait()
                if pil_img:
                    # Create ImageTk on MAIN THREAD
                    img_tk = ImageTk.PhotoImage(pil_img)
                    self.cache[(path, size_req)] = img_tk
                    callback(img_tk)
        except queue.Empty:
            pass

    def stop(self):
        self.running = False

# Global instance
loader = AsyncImageLoader()
