PROTOC_DIR = ../external/protobuf-2.5.0/
PROTOC     = $(PROTOC_DIR)src/protoc

PROTO_NET_DIR = ../external/protobuf-net/
PROTO_NET     = $(PROTO_NET_DIR)/ProtoGen/protogen.exe

MONO = mono

PROTO_FILES := $(wildcard *.proto)
PB_FILES := $(patsubst %.proto, %.pb, $(PROTO_FILES))
CS_FILES := $(patsubst %.proto, %.cs, $(PROTO_FILES))

all: protoc $(CS_FILES)

protoc:
	if test	-x $(PROTOC);\
	then echo ""; \
	else cd $(PROTOC_DIR) && ./configure && $(MAKE); fi

SUFFIXES: .pb .cs .proto

%.pb: %.proto
	$(PROTOC) $< --descriptor_set_out=$@

%.cs: %.pb
	$(MONO) $(PROTO_NET) -i:$< -o:$@

clean:
	rm *.pb *.cs
